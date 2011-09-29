
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Deployment.Compression.Zip;

namespace RAMBase
{
  internal partial class Persistor<T>
  {
    private void persistThread()
    {
      while (_isActive) {
        lock (_checkLock)
          if (!Monitor.Wait(_checkLock)) return;

        using (var scope = _container.CreateReadLockedScope()) {
          var tempFilename = getTemporaryWriteFilename();
          string checksum;

          using (var fs = new FileStream(tempFilename, FileMode.Create))
          using (var cs = new CrcStream(fs)) {
            _settings.Serializer.Invoke(scope.Model, cs);
            cs.Flush();

            checksum = getChecksumStr(cs.Crc);

            cs.Close();
            fs.Close();
          }


          File.Copy(tempFilename, getNewPrimaryFilename(checksum), overwrite: true);
          File.Copy(tempFilename, getNextFilename(checksum), overwrite: true);
          File.Delete(tempFilename);
          cleanupCheck();
        }
      }
    }

    private T loadPrimaryFile()
    {
      var filename = getPrimaryPersistenceFilename();
      string checksum = getChecksumStr(filename);

      using (var fs = File.OpenRead(filename))
      using (var cs = new CrcStream(fs)) {
        var model = _settings.Deserializer.Invoke(cs);

        // TODO: Actually should throw an error instead?
        if (getChecksumStr(filename) != getChecksumStr(cs.Crc))
          return default(T);

        cs.Close();
        fs.Close();

        return model;
      }
    }


    private string getTemporaryWriteFilename()
    {
      return Path.Combine(_settings.PersistFolder, "saving");
    }

    private string getPrimaryPersistenceFilename()
    {
      return Directory
        .GetFiles(_settings.PersistFolder, "save-primary-*")
        .FirstOrDefault();
    }

    private string getNewPrimaryFilename(string checksum)
    {
      return Path.Combine(_settings.PersistFolder, string.Format(
        "save-primary-{0}", checksum));
    }

    private string getNextFilename(string checksum)
    {
      var now = DateTime.Now;
      return Path.Combine(_settings.PersistFolder, string.Format(
        "save-{0:0000}{1:00}{2:00}-{3:00}{4:00}-{5}",
        now.Year, now.Month, now.Day, now.Hour, now.Minute, checksum));
    }

    private void cleanupCheck()
    {
      // TODO: implement
    }


    private string getChecksumStr(uint checksum)
    {
      return Convert.ToBase64String(BitConverter.GetBytes(checksum));
    }

    private string getChecksumStr(string filename)
    {
      var arr = filename.Split('-');
      return arr[arr.Length - 1];
    }

  }
}
