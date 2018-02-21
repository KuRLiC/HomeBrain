using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PTH = System.IO.Path;
using FLE = System.IO.File;

namespace HomeBrain
{
    public class FolderItem
    {
        private string _path;
        public string Path { get { return _path; } }
        public FolderItem(string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException();
            _path = path;
            Folder = new FolderExplorer(this);
            File = new FileExplorer(this);
        }
        public IEnumerable<string> FileNames { get { return Directory.EnumerateFiles(_path).Select(x => PTH.GetFileName(x)); } }
        public IEnumerable<string> FolderNames { get { return Directory.EnumerateDirectories(_path).Select(x => PTH.GetFileName(x)); } }
        public IEnumerable<FolderItem> Folders { get { return FolderNames.Select(x => new FolderItem(PTH.Combine(_path, x))); } }
        public IEnumerable<FileItem> Files { get { return FileNames.Select(x => new FileItem(PTH.Combine(_path, x))); } }
        public FolderExplorer Folder { get; protected set; }
        public FileExplorer File { get; protected set; }
        public bool FileExists(string name) { return FLE.Exists(PTH.Combine(_path, name)); }
        public bool FolderExists(string name) { return Directory.Exists(PTH.Combine(_path, name)); }
        public FolderItem GetFolder(string name) { return Directory.EnumerateDirectories(_path, name).Select(x => new FolderItem(x)).FirstOrDefault(); }
        public FolderItem CreateFolder(string name) { return new FolderItem(Directory.CreateDirectory(PTH.Combine(_path, name)).FullName); }
        public override string ToString()
        {
            return Path;
        }
    }

    public class FolderExplorer
    {
        private FolderItem _folder;
        public string Path { get { return _folder.Path; } }
        public FolderExplorer(FolderItem folder)
        {
            if (folder == null) throw new ArgumentNullException();
            _folder = folder;
        }
        public FolderItem this[string name]
        {
            get
            {
                return _folder.FolderExists(name) ? _folder.GetFolder(name) : _folder.CreateFolder(name);
            }
        }
        public override string ToString()
        {
            return Path;
        }
    }

    public class FileItem
    {
        private string _path;
        public string Path { get { return _path; } }

        public FileItem(string path)
        {
            this._path = path;
        }
        public byte[] Bytes { get { return File.ReadAllBytes(_path); } set { File.WriteAllBytes(_path, value); } }
        public string Text { get { return File.ReadAllText(_path); } set { File.WriteAllText(_path, value); } }
        public IEnumerable<string> Lines { get { return File.ReadLines(_path); } set { File.WriteAllLines(_path, value); } }
        public bool Exists { get { return File.Exists(_path); } }
        public FileStream Read { get { return File.OpenRead(_path); } }
        public FileStream Write { get { return File.OpenWrite(_path); } }
        public StreamReader TextReader { get { return new StreamReader(_path); } }
        public StreamWriter TextWriter { get { return new StreamWriter(_path); } }
        public BinaryReader BinReader { get { return new BinaryReader(Read); } }
        public BinaryWriter BinWriter { get { return new BinaryWriter(Write); } }
        public FileItem WriteLine()
        {
            using (var writer = TextWriter)
                writer.WriteLine();
            return this;
        }
        public FileItem WriteLine(string fmt, params object[] args)
        {
            using (var writer = TextWriter)
                writer.WriteLine(fmt, args);
            return this;
        }
        public override string ToString()
        {
            return Path;
        }
    }

    public class FileExplorer
    {
        private FolderItem _folder;
        public string Path { get { return _folder.Path; } }
        public FileExplorer(FolderItem folder)
        {
            if (folder == null) throw new ArgumentNullException();
            _folder = folder;
        }
        public FileItem this[string name]
        {
            get
            {
                return new FileItem(PTH.Combine(_folder.Path, name));
            }
        }
        public override string ToString()
        {
            return Path;
        }
    }
}
