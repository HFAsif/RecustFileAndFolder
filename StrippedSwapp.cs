public static void RecustFileAndFolderThree(string folderpath)
{
    if (IsNullOrWhiteSpace(folderpath) && !Directory.Exists(folderpath))
    {
        Debugger.Break();
        throw new ArgumentNullException(nameof(folderpath));
        var folderNewPath = "C:\\VSLatestSymbols";
    }
    var _Folders = Directory.GetDirectories(folderpath, "*", System.IO.SearchOption.AllDirectories);

    for (var i = _Folders.Length - 1; i > 0; i--)
    {
        var directory = new DirectoryInfo(_Folders[i]);
        var directories = directory.GetDirectories();

        foreach (var PdbSignedDir in directories)
        {
            var PdbStrppedDirs = PdbSignedDir.GetDirectories();

            foreach (var PdbStrppedDir in PdbStrppedDirs)
            {
                var sourceFileName = Path.Combine(PdbStrppedDir.FullName, directory.Name);

                if (File.Exists(sourceFileName))
                {
                    if (PdbStrppedDir.Name is "stripped" && PdbStrppedDir.GetFiles().ToList().FindAll(a => a.Extension == ".pdb").Count == 0)
                    {
                        DeleteDir(PdbStrppedDir.FullName, true);
                    }
                    else if (PdbStrppedDir.Name is "stripped" && PdbStrppedDir.GetFiles().ToList().FindAll(a => a.Extension == ".pdb").Count == 1)
                    {
                        var destFileName = Path.Combine(PdbStrppedDir.Parent.FullName, Path.GetFileName(sourceFileName));

                        File.Move(sourceFileName, destFileName);

                        DeleteDir(PdbStrppedDir.FullName, true);
                    }
                    else if (PdbStrppedDir.Name is "stripped" && PdbStrppedDir.GetFiles().ToList().FindAll(a => a.Extension == ".pdb").Count > 1)
                    {
                        Debugger.Break();
                        throw new InvalidOperationException("please take a look at pdb folders ");
                    }
                }

            }
        }
    }
}

public static void RecustFileAndFolder(string folderpath)
{
    if (IsNullOrWhiteSpace(folderpath) && !Directory.Exists(folderpath))
    {
        Debugger.Break();
        throw new ArgumentNullException(nameof(folderpath));
        var folderNewPath = "C:\\VSLatestSymbols";
    }
    //C:\\SymbolFiles\\WindowsCodecs.pdb\\EAE1253DBDF1015497E086409850476A1
    var TotalFolders = Directory.GetDirectories(folderpath, "*", System.IO.SearchOption.AllDirectories);
    //F:\\SymbolCache\\srvcli.pdb\\9B2D4D1514F7A3653BCA5EAFB4C3C4351\\stripped
    //string[] folders = System.IO.Directory.GetDirectories(@"F:\SymbolCache\", "*", System.IO.SearchOption.AllDirectories);

    var PdfFileList = new List<string>();

    foreach (var folder in TotalFolders)
    {
        var TotalFiles = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);
        if (TotalFiles.Length == 0) continue;

        //var Folderinfo = new DirectoryInfo(folder);

        foreach (var file in TotalFiles)
        {
            var fileinfo = new FileInfo(file);
            if(fileinfo.Exists)
            {
                var FileExtension = fileinfo.Extension;
                if (FileExtension == ".pdb")
                {
                    PdfFileList.Add(file);
                }
            }
        }
    }

    var AfterRemoveFolderList = Directory.GetDirectories(folderpath, "*", System.IO.SearchOption.AllDirectories);


    foreach (var folder in AfterRemoveFolderList)
    {
        if(!Directory.Exists(folder)) continue;
        var TotalFiles = Directory.GetFiles(folder, "*", SearchOption.AllDirectories);

        if (TotalFiles.Length == 0)
        {
            Directory.Delete(folder, true);
            Console.WriteLine("deleted {0}", folder); // Success
        }
    }

    for (int i = 0; i < PdfFileList.Count; i++)
    {
        var pdffile = PdfFileList[i];
        if (!File.Exists(pdffile)) continue;

        if (Path.GetExtension(pdffile) != ".pdb") continue;

        var dirinfo = new DirectoryInfo(pdffile);

        var destfilename = Path.Combine(new DirectoryInfo(dirinfo.Parent.FullName).Parent.FullName, Path.GetFileName(pdffile));
        //var parentdir = dirinfo.Parent.FullName;
        //var dirname = dirinfo.Name;


        if (dirinfo.Parent.Name == "stripped")
        {
            File.Move(pdffile, destfilename);
            Directory.Delete(dirinfo.Parent.FullName, true);
            Console.WriteLine("deleted {0}", dirinfo.Parent.FullName); // Success
        }
            


    }

    //var GetEmtyFolders = Directory.GetDirectories(NonPdfFolders)

    //var dir = Directory.GetDirectories(folderpath, "*", System.IO.SearchOption.AllDirectories);
    //for (int i = 0; i < dir.Length; i++)
    //{
    //    string folder = dir[i];
    //    var files = Directory.GetFiles(folder);
    //    if (files.Length == 0) continue;
    //    for (int j = 0; j < files.Length; j++)
    //    {
    //        var file = files[j];
    //        var filetension = Path.GetExtension(file);

    //        if (filetension == ".pdb")
    //        {
    //            var dirinfo = new DirectoryInfo(folder);
    //            var subdir = dirinfo.Parent.FullName;
    //            //var fileinfo = new FileInfo(file);
    //            //fileinfo.MoveTo(subdir, "");

    //            //var from = Path.GetDirectoryName(file);
    //            //var to = subdir;

    //            if (File.Exists(file))
    //            {
    //                var combined = Path.Combine(subdir, Path.GetFileName(file));
    //                //var folderName = Path.GetFullPath(file);


    //                var directorypath = Path.GetDirectoryName(file);


    //                DirectoryInfo directoryInfo = new DirectoryInfo(directorypath);
    //                string foldername = directoryInfo.Name;

    //                if (foldername == "stripped")
    //                {
    //                    if (File.Exists(combined) /*&& !FilesAreEqual(new FileInfo(file), new FileInfo(combined))*/)
    //                    {
    //                        File.Delete(combined);
    //                        Console.WriteLine("deleted {0}", dirinfo); // Success
    //                    }
    //                    File.Move(file, combined);
    //                    Console.WriteLine("moved {0}", dirinfo); // Success

    //                    //Console.WriteLine(directoryInfo.GetFiles().Length);

    //                    var findfile = directoryInfo.GetFiles().ToList().Find(a => a.FullName == file);

    //                    if(findfile == null)
    //                    {
    //                        //File.Delete(combined);
    //                        //directoryInfo.Delete();
    //                        Directory.Delete(directoryInfo.FullName, true);
    //                    }
    //                }

    //                var strippedfolder = Path.Combine(directoryInfo.FullName, "stripped");

    //                if (Directory.Exists(strippedfolder))
    //                {
    //                    Directory.Delete(strippedfolder, true);

    //                    Console.WriteLine("deleted {0}", strippedfolder);
    //                }
    //                //if (Directory.Exists(strippedfolder))
    //                //{
    //                //    if (File.Exists(combined) /*&& !FilesAreEqual(new FileInfo(file), new FileInfo(combined))*/)
    //                //    {
    //                //        File.Delete(combined);
    //                //        Console.WriteLine("deleted {0}", dirinfo); // Success
    //                //    }
    //                //    File.Move(file, combined);
    //                //    Console.WriteLine("moved {0}", dirinfo); // Success
    //                //}

    //                    //    if (foldername.Contains("stripped"))
    //                    //{
    //                    //    if (File.Exists(combined) /*&& !FilesAreEqual(new FileInfo(file), new FileInfo(combined))*/)
    //                    //    {
    //                    //        File.Delete(combined);
    //                    //        Console.WriteLine("deleted {0}", dirinfo); // Success
    //                    //    }
    //                    //    File.Move(file, combined);
    //                    //    Console.WriteLine("moved {0}", dirinfo); // Success

    //                    //    //if (File.Exists(file) && FilesAreEqual(new FileInfo(file), new FileInfo(combined)))
    //                    //    //{
    //                    //    //    dirinfo.Delete(true);
    //                    //    //    Console.WriteLine("deleted {0}", dirinfo); // Success
    //                    //    //}
    //                    //    //else
    //                    //    //{
    //                    //    //    File.Move(file, combined); // Try to move
    //                    //    //    dirinfo.Delete(true);
    //                    //    //    Console.WriteLine("deleted {0}", dirinfo); // Success
    //                    //    //}

    //                    //}
    //            }
    //        }
    //    }


    //}


    //foreach (var subfolder in dir)
    //{
    //    var symdir = Directory.GetDirectories(subfolder);
    //    foreach (var subdir in symdir)
    //    {
    //        var subdir2 = Directory.GetDirectories(subdir);
    //        if (subdir2.Count() == 1)
    //        {
    //            var getPDB = Directory.GetFiles(subdir);
    //            foreach (var pdb in getPDB)
    //            {
    //                var extension = Path.GetExtension(pdb);
    //                if (extension == ".error")
    //                {

    //                }
    //                Console.WriteLine(extension);
    //            }
    //        }

    //    }
    //}
}
