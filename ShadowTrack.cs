using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace AutoCASS {

    public class QueueItem {
        public ObjectId _id { get; set; }
        public ObjectId metadata { get; set; }
        public string script { get; set; }
        public string script_name { get; set; }
        public string presort { get; set; }
        public string presort_name { get; set; }
        public string pwk { get; set; }
        public string pwk_name { get; set; }
        public string db { get; set; }
        public string db_name { get; set; }
        public string jobid { get; set; }
        public string duedate { get; set; }
        public string csr_email { get; set; }
        public string csr2_email { get; set; }
        public bool lightning { get; set; }
        public bool lightning_print { get; set; }
        public string scr_ini { get; set; }
        public string pre_ini { get; set; }
        public string pwk_ini { get; set; }
        public string cmd_ini { get; set; }
        public string cmd_name { get; set; }
        public bool complete { get; set; }
        public bool inProgress { get; set; }
        public int qty { get; set; }
        public DateTime modified { get; set; }
        public DateTime created { get; set; }
    }

    class AutoCASS {
        // fields
        static string jobid;
        static string server;
        static MongoClient client;
        static IMongoDatabase database;
        static QueueItem curItem;
        static IMongoCollection<BsonDocument> queueCollection;
        static IMongoCollection<BsonDocument> ordersCollection;
        static StreamWriter jobid_log;
        static ObjectId data_id;
        static string data_name;
        static bool is_live;

        static void Main(string[] args) { 
            using (var mutex = new SingleGlobalInstance(1000)) {


                // If there are any files currently in the hot folder, quit immediately. There's no work to be done.
                var curFiles = new DirectoryInfo(@"D:\mhWork\_AUTO\_Cass\accuzip");
                if (!mutex._hasHandle || curFiles.GetFiles().Length > 0) {
                    Debug.WriteLine("in progress already...");
                    return;
                }

                //server = "69.64.76.11:37017"; var serverCompVal = "2"; var is_live = false;
                //DEV
                //server = "69.64.76.11:37017"; var serverCompVal = "2"; is_live = false;
                //LIVE
                server = "64.150.187.109:37017"; var serverCompVal = "3"; is_live = true;

                client = new MongoClient("mongodb://" + server);

                database = client.GetDatabase("orders-mvp");
                queueCollection = database.GetCollection<BsonDocument>("autocass_queue");

                ordersCollection = database.GetCollection<BsonDocument>("orders");

                IMongoCollection<BsonDocument> resourceCollection = database.GetCollection<BsonDocument>("resources.files");
                GridFSBucket resourceBucket = new GridFSBucket(database, new GridFSBucketOptions { BucketName = "resources" });

                IMongoCollection<BsonDocument> uploadCollection = database.GetCollection<BsonDocument>("uploads.files");
                GridFSBucket uploadBucket = new GridFSBucket(database, new GridFSBucketOptions { BucketName = "uploads" });

                var aliases = new string[4];
                UpdateResult updateResult;
                string csr_email = "";
                string csr2_email = "";
                string job_name = "";
                string company = "";

                // Try upload
                try {
                    // Check if there are any completed jobs in the outgoingJOB folder
                    string outgoingDir = @"D:\mhWork\_AUTO\_Cass\_outgoingJOB\";
                    var jobsDone = Directory.GetDirectories(@"D:\mhWork\_AUTO\_Cass\_outgoingJOB\");
                    // Because the upload and download are both done in this script sequentially, there is guaranteed to be either 0 or 1 in the completed folder.
                    if (jobsDone.Length == 1) {
                        // Get jobid from folder path
                        jobid = jobsDone[0].Substring(jobsDone[0].LastIndexOf(@"\") + 1);

                        // Start log with timestamp...
                        jobid_log = File.AppendText(@"D:\mhWork\_AUTO\_Cass\logs\" + jobid + ".txt");
                        jobid_log.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss - "));

                        // Get current queue item, set csr_email values in case something goes wrong early
                        var uploadFilter = Builders<BsonDocument>.Filter.Eq("jobid", jobid);
                        var queue_result = findInCollection(uploadFilter, queueCollection, true);
                        if (queue_result != null) {
                            curItem = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<QueueItem>(queue_result);
                            csr_email = curItem.csr_email;
                            csr2_email = curItem.csr2_email;
                        } else {
                            csr_email = "";
                            csr2_email = "";
                        }
                        jobid_log.WriteLine("CSR 1: " + csr_email);
                        jobid_log.WriteLine("CSR 2: " + csr2_email);

                        // Pull order from database
                        var order = findInCollection(Builders<BsonDocument>.Filter.Eq("job_id", jobid), ordersCollection, false);
                        job_name = order.GetElement("name").Value.ToString();
                        company = order.GetElement("company").Value.ToBsonDocument().GetElement(serverCompVal).Value.ToString();

                        // Attempt upload of Data Print File
                        aliases[0] = uploadFileToGrid(outgoingDir + jobid + @"\N" + jobid + @"final.csv", uploadCollection, uploadBucket, "data");

                        // Zip, then attempt upload of Presort folder
                        ZipFile.CreateFromDirectory(outgoingDir + jobid + @"\Presort Folder\", outgoingDir + jobid + @"\presort.zip");
                        while (!File.Exists(outgoingDir + jobid + @"\presort.zip")) {
                            System.Threading.Thread.Sleep(1000);
                        }
                        aliases[1] = uploadFileToGrid(outgoingDir + jobid + @"\presort.zip", uploadCollection, uploadBucket, "presort");

                        // Zip, then attempt upload of Maildat folder
                        ZipFile.CreateFromDirectory(outgoingDir + jobid + @"\MailDat\", outgoingDir + jobid + @"\MailDat.zip");
                        while (!File.Exists(outgoingDir + jobid + @"\MailDat.zip")) {
                            System.Threading.Thread.Sleep(1000);
                        }
                        var maildatPath = outgoingDir + jobid + @"\MailDat.zip";

                        var maildatInfo = new FileInfo(maildatPath);
                        sendRichardMaildat(company, jobid, maildatPath, is_live);
                        System.Threading.Thread.Sleep(3000);
                        while (fileIsLocked(maildatInfo)) {
                            System.Threading.Thread.Sleep(1000);
                            //add a log counter here to find out how long the actual delay is.
                        }

                        aliases[2] = uploadFileToGrid(maildatPath, uploadCollection, uploadBucket, "maildat");

                        // Upload DidNotMail.csv to Suppression tab
                        var dnm_path = outgoingDir + jobid + @"\DidNotMail.csv";
                        if (File.Exists(dnm_path)) {
                            aliases[3] = uploadFileToGrid(dnm_path, uploadCollection, uploadBucket, "suppression");
                        } else {
                            aliases[3] = "";
                        }

                        // After upload, delete all folders from outgoingJOB, they are no longer needed
                        var finalDir = new DirectoryInfo(outgoingDir);
                        foreach (FileInfo file in finalDir.GetFiles()) {
                            file.Delete();
                        }
                        foreach (DirectoryInfo dir in finalDir.GetDirectories()) {
                            dir.Delete(true);
                        }

                        // Update job to show automation is complete
                        var updateFilter = Builders<BsonDocument>.Filter.Eq("job_id", jobid);
                        var update = Builders<BsonDocument>.Update.Set("prepping_data", false).Set("dataPrepComplete", true).Set("data_done", DateTime.Now);
                        updateResult = ordersCollection.UpdateOne(updateFilter, update);

                        // Update item in queue, indicating that it has been processed
                        var setCompleteFilter = Builders<BsonDocument>.Filter.Eq("inProgress", true) & Builders<BsonDocument>.Filter.Eq("jobid", jobid);
                        var updateQueue = Builders<BsonDocument>.Update.Set("complete", true).Set("inProgress", false).CurrentDate("modified");
                        updateResult = queueCollection.UpdateOne(setCompleteFilter, updateQueue);

                        if (curItem != null) {
                            // Delete Command, as it is already saved in the database and is just taking up space here
                            /*if (curItem.lightning) {
                                var cmd_path = @"C:\Program Files (x86)\AccuZIP6 5.0\Commands\_" + curItem.cmd_name + ".txt";
                                if (File.Exists(cmd_path))
                                {
                                    File.Delete(cmd_path);
                                }
                            }*/

                            // If job is marked as Lightning and is set to do AutoPrint immediately, mark the AutoPrint Queue item as "ready" and put in the appropriate Mongo ID for the data print file.
                            if (curItem.lightning && curItem.lightning_print) {
                                var printQueueCollection = database.GetCollection<BsonDocument>("autoprint_queue");
                                var printFilter = Builders<BsonDocument>.Filter.Eq("jobid", jobid) &
                                    Builders<BsonDocument>.Filter.Eq("ready", false) &
                                    Builders<BsonDocument>.Filter.Eq("complete", false) &
                                    Builders<BsonDocument>.Filter.Eq("in_progress", false);
                                var printUpdate = Builders<BsonDocument>.Update.Set("data_id", data_id.ToString()).Set("data_name", data_name).Set("ready", true);
                                printQueueCollection.UpdateOne(printFilter, printUpdate);

                            }

                        }


                        // Report successful upload in log for the previously printed timestamp
                        jobid_log.WriteLine("Upload successful!");
                        jobid_log.WriteLine();
                        successMail(jobid, job_name, company, csr_email, csr2_email, is_live);

                        // After everything is uploaded and updated, update the UploadIcons array in the job for each file that was actually changed.
                        var iconFilter = Builders<BsonDocument>.Filter.Eq("job_id", jobid);
                        UpdateDefinition<BsonDocument> iconUpdate;
                        for (int i = 0; i < 4; i++) {
                            if (aliases[i] != "") {
                                iconUpdate = Builders<BsonDocument>.Update.Push("UploadIcons", aliases[i]);
                                updateResult = ordersCollection.UpdateOne(iconFilter, iconUpdate);
                                jobid_log.WriteLine("Icon updated: " + aliases[i]);
                            }
                        }

                    } else if (jobsDone.Length > 1) {
                        foreach (var job in jobsDone) {
                            var tmp_jobid = job.Substring(job.LastIndexOf(@"\") + 1);
                            var uploadFilter = Builders<BsonDocument>.Filter.Eq("jobid", tmp_jobid);
                            var queue_result = findInCollection(uploadFilter, queueCollection, true);
                            string tmp_csr_email = "";
                            string tmp_csr2_email = "";
                            if (queue_result != null) {
                                curItem = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<QueueItem>(queue_result);
                                tmp_csr_email = curItem.csr_email;
                                tmp_csr2_email = curItem.csr2_email;
                            }
                            failureMail(tmp_jobid, "upload", "AutoCASS attempted to upload multiple jobs at once, and failed. Please re-run job " + tmp_jobid + ".", is_live, tmp_csr_email, tmp_csr2_email);
                            var delete_filter = Builders<BsonDocument>.Filter.Eq("jobid", tmp_jobid);
                            queueCollection.DeleteMany(delete_filter);
                        }
                        throw new Exception("AutoCASS attempted to upload multiple jobs at once, and failed. Please re-run job " + jobid + ".");
                    }
                } catch (System.Net.Mail.SmtpException e) {

                    jobid_log.WriteLine("----------SMTP ERROR----------");
                    // After everything is uploaded and updated, update the UploadIcons array in the job for each file that was actually changed.
                    var iconFilter = Builders<BsonDocument>.Filter.Eq("job_id", jobid);
                    UpdateDefinition<BsonDocument> iconUpdate;
                    for (int i = 0; i < 3; i++) {
                        if (aliases[i] != "") {
                            iconUpdate = Builders<BsonDocument>.Update.Push("UploadIcons", aliases[i]);
                            updateResult = ordersCollection.UpdateOne(iconFilter, iconUpdate);
                            jobid_log.WriteLine("Icon updated: " + aliases[i]);
                        }
                    }

                    successMail(jobid, job_name, company, csr_email, csr2_email, is_live);

                } catch (Exception e) {

                    // Report failure in log, send email to admins
                    Debug.WriteLine("Exception:::::::: " + e.ToString());
                    if (jobid_log != null) {
                        jobid_log.WriteLine("UPLOAD FAILED! Exception: " + e.ToString());
                        jobid_log.WriteLine();
                    }
                    if (jobid != null) {
                        failureMail(jobid, "upload", e.ToString(), is_live, csr_email, csr2_email);
                    }

                    // Delete failed folders from outgoingJOB

                    var outgoingDir = new DirectoryInfo(@"D:\mhWork\_AUTO\_Cass\_outgoingJOB");
                    if (is_live) {
                        foreach (FileInfo file in outgoingDir.GetFiles()) {
                            file.Delete();
                        }
                        foreach (DirectoryInfo dir in outgoingDir.GetDirectories()) {
                            dir.Delete(true);
                        }

                        var filter = Builders<BsonDocument>.Filter.Eq("jobid", jobid);
                        queueCollection.DeleteMany(filter);
                    }

                }

                if (jobid_log != null) {
                    jobid_log.Close();
                }

                // Try download
                try {
                    // Search for incomplete job that has the most recent duedate in the queue
                    var filter = Builders<BsonDocument>.Filter.Eq("complete", false) & Builders<BsonDocument>.Filter.Eq("inProgress", false);
                    var sort = Builders<BsonDocument>.Sort.Ascending("duedate");

                    var result = findInCollection(filter, queueCollection, true);
                    if (result != null) {
                        // Get jobid from queue item
                        curItem = MongoDB.Bson.Serialization.BsonSerializer.Deserialize<QueueItem>(result);
                        jobid = curItem.jobid;

                        // Update job to show data automation is starting
                        var updateFilter = Builders<BsonDocument>.Filter.Eq("job_id", jobid);
                        var update = Builders<BsonDocument>.Update.Set("data_start", DateTime.Now);
                        updateResult = ordersCollection.UpdateOne(updateFilter, update);

                        // Write timestamp to log
                        jobid_log = File.AppendText(@"D:\mhWork\_AUTO\_Cass\logs\" + jobid + ".txt");
                        jobid_log.Write(DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss - "));

                        // If this is a lightning job, pull Script, Presort, and Paperwork from the queue item, and put them into tmp files.
                        // If it is NOT a lightning job, download everything from the Resources bucket.
                        // For both, download the CSV from the Uploads bucket.
                        string[] filenames = new string[5];

                        if (curItem.lightning) {
                            filenames[0] = Path.Combine(Path.GetTempPath(), "scr.txt");
                            using (StreamWriter sw = new StreamWriter(filenames[0])) {
                                sw.Write(curItem.scr_ini);
                            }
                            filenames[1] = Path.Combine(Path.GetTempPath(), "pre.txt");
                            using (StreamWriter sw = new StreamWriter(filenames[1])) {
                                sw.Write(curItem.pre_ini);
                            }
                            filenames[2] = Path.Combine(Path.GetTempPath(), "pwk.txt");
                            using (StreamWriter sw = new StreamWriter(filenames[2])) {
                                sw.Write(curItem.pwk_ini);
                            }
                            filenames[4] = Path.Combine(Path.GetTempPath(), "cmd.txt");
                            using (StreamWriter sw = new StreamWriter(filenames[4])) {
                                sw.Write(curItem.cmd_ini);
                            }
                        } else {
                            // Script
                            filenames[0] = downloadFromGrid(curItem.script, resourceCollection, resourceBucket);
                            // Presort
                            filenames[1] = downloadFromGrid(curItem.presort, resourceCollection, resourceBucket);
                            // Paperwork
                            filenames[2] = downloadFromGrid(curItem.pwk, resourceCollection, resourceBucket);
                        }

                        // Data file
                        filenames[3] = downloadFromGrid(curItem.db, uploadCollection, uploadBucket);

                        string[] file_types = { "Script INI", "Presort INI", "Paperwork INI", "Data Print File" };
                        List<string> corrupt = new List<string>();
                        List<string> nochunk = new List<string>();

                        for (int j = 0; j < 4; j++) {
                            if (filenames[j] == "xxx") {
                                corrupt.Add(file_types[j]);
                            }
                            if (filenames[j] == "GridError") {
                                nochunk.Add(file_types[j]);
                            }
                        }
                        if (corrupt.Count > 0 || nochunk.Count > 0) {
                            string message = "";
                            if (corrupt.Count > 0) {
                                message += "The following files were not found in the GridFS database. Please delete and re-upload the files to the job board.\n\n";
                                message += string.Join(", ", corrupt);
                                message += "\n\n";
                            }
                            if (nochunk.Count > 0) {
                                message += "The following files are corrupt or have no content/chunks saved. Please delete and re-upload the files to the job board.";
                                message += string.Join(", ", nochunk);
                                message += "\n\n";
                            }
                            throw new Exception(message);
                        }

                        var duedate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                        duedate = duedate.AddSeconds(Convert.ToDouble(curItem.duedate)).ToLocalTime();
                        var duedateString = duedate.ToString("MM/dd/yyyy");

                        // Modify script file to replace "<dMailDate>" with the job's due date
                        jobid_log.WriteLine("Adding date: " + duedate);

                        string text = File.ReadAllText(filenames[0]);
                        text = text.Replace("\"<dMailDate>\"", duedateString);
                        text = text.Replace("\"CurrentDate\"", duedateString);    // Yes, I know it says "current date", it really means due date
                        File.WriteAllText(filenames[0], text);

                        // Delete existing AccuZIP files
                        File.Delete(@"C:\Program Files (x86)\AccuZIP6 5.0\Scripts\_automh.scr");
                        File.Delete(@"C:\Program Files (x86)\AccuZIP6 5.0\Scripts\Presorts\_automh_presort.ini");
                        File.Delete(@"C:\Program Files (x86)\AccuZIP6 5.0\Scripts\Presorts\Statements\_automh_pwk.ini");

                        var dbDir = new DirectoryInfo(@"D:\mhWork\_AUTO\_Cass\accuzip");
                        int i;
                        foreach (FileInfo file in dbDir.GetFiles()) {
                            i = 0;
                            while (fileIsLocked(file)) {
                                System.Threading.Thread.Sleep(1000);
                                i++;
                                if (i > 60) throw new Exception("Files in working directory took too long to delete. Try running job again.");
                            }
                            file.Delete();
                        }


                        // Move new files to their proper locations
                        File.Move(filenames[0], @"C:\Program Files (x86)\AccuZIP6 5.0\Scripts\_automh.scr");
                        File.Move(filenames[1], @"C:\Program Files (x86)\AccuZIP6 5.0\Scripts\Presorts\_automh_presort.ini");
                        File.Move(filenames[2], @"C:\Program Files (x86)\AccuZIP6 5.0\Scripts\Presorts\Statements\_automh_pwk.ini");

                        // If running a lightning job that has a non-empty command, put that in the proper location.
                        if (curItem.lightning && filenames[4] != null && new FileInfo(filenames[4]).Length != 0) {
                            var cmd_path = @"C:\Program Files (x86)\AccuZIP6 5.0\Commands\_" + curItem.cmd_name + ".txt";
                            if (File.Exists(cmd_path)) {
                                File.Delete(cmd_path);
                            }
                            File.Move(filenames[4], cmd_path);
                        }

                        // Move database file to hot folder, triggering the start of the AccuZIP script
                        var dbPath = Path.Combine(@"D:\mhWork\_AUTO\_Cass\accuzip\" + jobid.ToString() + @".csv");
                        File.Move(filenames[3], dbPath);

                        // Update queue item to mark that it is in progress
                        updateFilter = Builders<BsonDocument>.Filter.Eq("_id", curItem._id);
                        update = Builders<BsonDocument>.Update.Set("inProgress", true).CurrentDate("modified");
                        updateResult = queueCollection.UpdateOne(updateFilter, update);

                        // Write message of completion to log
                        jobid_log.WriteLine("Download successful!");
                        jobid_log.WriteLine();
                    }
                } catch (Exception e) {
                    // Report failure to log, email admins
                    Debug.WriteLine("Exception:::::::: " + e.ToString());
                    jobid_log.WriteLine("DOWNLOAD FAILED! Exception: " + e.ToString());
                    jobid_log.WriteLine();
                    failureMail(jobid, "download", e.ToString(), is_live, csr_email, csr2_email);

                    var filter = Builders<BsonDocument>.Filter.Eq("jobid", jobid) & Builders<BsonDocument>.Filter.Eq("complete", false);
                    queueCollection.DeleteMany(filter);


                    var printQueueCollection = database.GetCollection<BsonDocument>("autoprint_queue");
                    printQueueCollection.DeleteMany(filter);
                }

                if (jobid_log != null) {
                    jobid_log.Close();
                }
            }
        }

        /*
         * Check if file is currently unavailable, i.e. being used by another process
         * @param file FileInfo describing the file to be checked
         * @return True if file is locked, False if file is safe to be opened/deleted
         */
        private static bool fileIsLocked(FileInfo file) {
            FileStream stream = null;

            try {
                stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            } catch (IOException) {
                return true;
            } finally {
                if (stream != null) stream.Close();
            }

            return false;
        }

        /* Take a string representing an ObjectID in GridFS, and download that file into an existing or new
        directory based on the jobid. 
        @param idString The ObjectID string to be downloaded
        @param collection The Mongo collection to search within for the ObjectID
        @param bucket A GridFSBucket pointing toward the proper bucket within the collection.
        @returns A string representing the location the downloaded file was saved to on the disk.
        */
        static string downloadFromGrid(string idString, IMongoCollection<BsonDocument> collection, GridFSBucket bucket) {
            // Point toward the correct document in the database
            var id = new ObjectId(idString);
            var doc = findInCollection(Builders<BsonDocument>.Filter.Eq("_id", id), collection, false);

            // Get filename of document, and initialize global variables for uploading
            // Catch exception if the file isn't found
            var filename = "";
            try {
                filename = doc.GetElement("filename").Value.ToString();
            } catch (System.ArgumentOutOfRangeException e) {
                return "xxx";
            } catch (System.NullReferenceException e) {
                return "xxx";
            }

            // If jobid directory doesn't exist, create it. Then prepare to save file locally inside it.
            Directory.CreateDirectory(@"C:\Users\Public\Documents\Auto_AccuZip\" + jobid);
            var url = @"C:\Users\Public\Documents\Auto_AccuZip\" + jobid + @"\" + filename;

            // Download and save file, returning its local filesystem location. 
            Stream stream = new FileStream(url, FileMode.Create);

            try {
                bucket.DownloadToStream(id, stream);
            } catch (GridFSChunkException) {
                return "GridError";
            }
            stream.Close();
            return url;
        }

        /* Find an object in a collection based on a filter. Typically used to find by ID, or to
        search the Orders collection for the metadata belonging to a certain jobid. 
        @param filter The search conditions, usually matching id
        @param collection The Mongo collection to be searched within
        @returns  The BSONDocument result from the search
        */
        static BsonDocument findInCollection(FilterDefinition<BsonDocument> filter,
            IMongoCollection<BsonDocument> collection,
            bool sort) {
            IAsyncCursor<BsonDocument> cursor;
            // Create a cursor at the start of the Find results, and get the first result
            if (sort) {
                cursor = collection.Find(filter).Sort(Builders<BsonDocument>.Sort.Ascending("duedate")).Sort(Builders<BsonDocument>.Sort.Descending("lightning")).ToCursor();
            } else {
                cursor = collection.Find(filter).ToCursor();
            }

            cursor.MoveNext();
            try {
                return cursor.Current.ElementAt(0);
            } catch (ArgumentOutOfRangeException) {
                return null;
            }
        }

        /* Find the metadata for the jobid of the current job.
        @param jobid A string containing the jobid of the current job.
        @returns A string representation of the ObjectID of the current job's metadata.
        */
        static string getMetadata(string jobid) {
            var collection = database.GetCollection<BsonDocument>("orders");
            var doc = findInCollection(Builders<BsonDocument>.Filter.Eq("job_id", jobid), collection, false);
            return doc.GetElement("_id").Value.ToString();
        }

        /* Upload a file to GridFS. This is now only used for the proof PDF.
        @param file The file to be uploaded to Grid.
        @param collection The Mongo collection the file will be uploaded to
        @param bucket The GridFSBucket pointing toward the proper bucket within the collection
        @param filetype "data" for Data Print File, "presort" for Presort Folder
        @returns A string containing the ObjectID of the final uploaded file.
        */
        static string uploadFileToGrid(string file, IMongoCollection<BsonDocument> collection, GridFSBucket bucket, string filetype) {
            try {
                string aliases;
                if (filetype == "data") {
                    aliases = "data_print_file";
                } else if (filetype == "presort") {
                    aliases = "postal_papers";
                } else if (filetype == "suppression") {
                    aliases = "suppression";
                } else {
                    aliases = "maildat";
                }

                // Upload result PDF to GridFS without any jobid, aliases, or anything like that
                // since for some reason we can't add it straightaway
                Stream stream = new FileStream(file, FileMode.Open);
                var upload_id = new ObjectId(bucket.UploadFromStream("temp", stream).ToString());
                stream.Close();

                // Now we can find the just-uploaded file, delete the previous one, add the rest of the data, then re-upload the full, final file.
                var doc = findInCollection(Builders<BsonDocument>.Filter.Eq("_id", upload_id), collection, false);
                collection.DeleteOne(new BsonDocument { { "_id", upload_id } });

                if (filetype == "presort") {
                    doc.Set("filename", jobid + "_presort.zip");
                    doc.Set("aliases", aliases);
                    doc.Set("contentType", "application/zip");
                } else if (filetype == "data") {
                    data_name = jobid + "_post_CASS.csv"; // Needed for Lightning AutoPrint
                    doc.Set("filename", data_name);
                    doc.Set("aliases", aliases);
                    doc.Set("contentType", "text/csv");
                } else if (filetype == "suppression") {
                    doc.Set("filename", jobid + "_did_not_mail.csv");
                    doc.Set("aliases", aliases);
                    doc.Set("contentType", "text/csv");
                } else {
                    doc.Set("filename", jobid + "_maildat.zip");
                    doc.Set("aliases", aliases);
                    doc.Set("contentType", "application/zip");
                }

                doc.Set("jobid", jobid);
                doc.Set("uploaded_by", "Automation");
                var metadata = new ObjectId(getMetadata(jobid));
                doc.Set("metadata", metadata);
                if (filetype == "data") data_id = upload_id;

                /*
                // Check if a data print file is already uploaded for this job. If so, use the same ObjectID. Otherwise, use the one from the first upload a few lines ago.
                var existing = findInCollection(Builders<BsonDocument>.Filter.Eq("jobid", jobid) & Builders<BsonDocument>.Filter.Eq("aliases", aliases), collection, false);
                if (existing == null)
                {
                    doc.Set("_id", tempId);
                    if (filetype == "data") data_id = tempId;       // Needed for Lightning AutoPrint
                }
                else
                { 
                    var existingId = new ObjectId(existing.GetElement("_id").Value.ToString());
                    doc.Set("_id", existingId);
                    if (filetype == "data") data_id = existingId;   // Needed for Lightning AutoPrint
                    collection.DeleteOne(new BsonDocument { { "_id", existingId } });
                }
                */

                // Check if file type currently being uploaded is going to replace any old files.
                BsonDocument existing;
                if (filetype == "suppression") {
                    existing = null;
                } else {
                    existing = findInCollection(Builders<BsonDocument>.Filter.Eq("jobid", jobid) & Builders<BsonDocument>.Filter.Eq("aliases", aliases), collection, false);
                    if (existing != null) {
                        var existing_id = new ObjectId(existing.GetElement("_id").Value.ToString());
                        collection.DeleteOne(new BsonDocument { { "_id", existing_id } });
                    }
                }

                collection.InsertOne(doc);

                // Mark badge count to be incremented if necessary
                if (existing == null) {
                    return aliases;
                }

                return "";
            } catch (Exception e) {
                throw e;
            }
        }

        /* 
         * Send an email to the system admins to inform of an automation failure
         * @param jobid The jobid of the job that had a failure
         * @param operation Either "upload" or "download", wherever the error happened
         * @param error The exception that was caught, printed out
         */
        static void failureMail(string jobid, string operation, string error, bool is_live, string csr_email, string csr2_email) {
            SmtpClient client = new SmtpClient("smtp.office365.com", 587);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("info@saltcreekmedia.com", "S@ltcr33k1!");
            client.EnableSsl = true;

            MailMessage message = new MailMessage();
            message.To.Add("eric@saltcreekmedia.com");
            if (is_live) {
                if (csr_email != "") {
                    message.To.Add(csr_email);
                }
                if (csr2_email != "") {
                    message.To.Add(csr2_email);
                }
                message.To.Add("josh@saltcreekmedia.com");
                message.To.Add("michael@saltcreekmedia.com");
                message.To.Add("richard@rsmail.com");
            }
            foreach (var r in addCompanyRecipients(jobid)) {
                message.To.Add(r);
            }
            message.From = new MailAddress("info@saltcreekmedia.com");
            message.Subject = "AUTOCASS ERROR: Job " + jobid + " has failed to " + operation;

            message.Body = "ERROR: Job " + jobid + " has failed to " + operation + " the appropriate files in the AutoCASS process.\n\n" + error;

            client.Send(message);
        }

        /* 
         * Send an email to the system admins to inform of an automation failure
         * @param jobid The jobid of the job that had a failure
         * @param operation Either "upload" or "download", wherever the error happened
         * @param error The exception that was caught, printed out
         */
        static void successMail(string jobid, string job_name, string company, string csr_email, string csr2_email, bool is_live) {
            SmtpClient client = new SmtpClient("smtp.office365.com", 587);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("info@saltcreekmedia.com", "S@ltcr33k1!");
            client.EnableSsl = true;

            MailMessage message = new MailMessage();
            if (is_live) {
                //message.To.Add("eric@saltcreekmedia.com");
                message.To.Add("michael@saltcreekmedia.com");
                message.To.Add("richard@rsmail.com");
                if (csr_email != "") {
                    message.To.Add(csr_email);
                }
                if (csr2_email != "") {
                    message.To.Add(csr2_email);
                }
            } else {
                message.To.Add("eric@saltcreekmedia.com");
            }
            foreach (var r in addCompanyRecipients(jobid)) {
                message.To.Add(r);
            }
            message.From = new MailAddress("info@saltcreekmedia.com");
            message.Subject = company + " Job " + jobid + " successfully CASS certified";
            message.Body = company + " Job " + jobid + " [" + job_name + "] has successfully completed the automated CASS certification, NCOA, and Presort process.\n\n\nIf you would like to stop receiving these emails, please contact eric@saltcreekmedia.com.";

            client.Send(message);
        }

        static void sendRichardMaildat(string company, string jobid, string path, bool is_live) {
            if (!is_live) return;

            SmtpClient client = new SmtpClient("smtp.office365.com", 587);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("info@saltcreekmedia.com", "S@ltcr33k1!");
            client.EnableSsl = true;

            Attachment maildat = new Attachment(path);

            using (MailMessage message = new MailMessage()) {
                message.To.Add("richard@rsmail.com");
                message.From = new MailAddress("info@saltcreekmedia.com");
                message.Subject = "Job " + jobid + " MailDat attached";
                message.Body = "The MailDat for " + company + " Job " + jobid + " is attached to this email.";
                message.Attachments.Add(maildat);

                client.Send(message);
            }
        }

        static List<string> addCompanyRecipients(string jobid)
        {
            List<String> to = new List<string>();

            var order = findInCollection(Builders<BsonDocument>.Filter.Eq("job_id", jobid), ordersCollection, false);
            int cid = order.GetElement("company_id").Value.ToInt32();

            if (is_live) {
                switch (cid) {
                    case 892:
                        to.Add("josh.smith@advancemarket.net");
                        to.Add("rickg@rsmail.com");
                        break;
                }
            } else {
                switch (cid) {
                    case 313:
                        to.Add("ericsbollinger@gmail.com");
                        break;
                }
            }

            return to;
        }
    }
}
