using System;
using System.Windows.Media;

namespace ClockApplication
{
    sealed class Settings
    {
        bool autoload, showSeconds = true, topMost = true;
        //#ffff5f9e - Brushes.CadetBlue
        byte fgA = 255, fgR = 95, fgG = 158, fgB = 160;
        //#ffadd8e6 - Brushes.LightBlue
        byte cfA = 255, cfR = 173, cfG = 216, cfB = 230;
        double x, y;

        public Settings()
        {
        }

        public bool IsDefault {
            get
            {
                string path;
#if DEBUG
                path = "settings.dat";

#else
                path = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Clocks");

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                path = System.IO.Path.Combine(path, "settings.dat");
#endif
                return !System.IO.File.Exists(path);
            }
        }
#region settings
        /// <summary>y - позиция окна при запуске программы</summary>
        public double Y
        {
            get { return y; }
            set
            {   
                y = value;
            }
        }

        /// <summary>x - позиция окна при запуске программы</summary>
        public double X
        {
            get { return x; }
            set
            {
                x = value;
            }
        }

        /// <summary>Показывать ли цифры</summary>
        public bool ShowSeconds {
            get { return showSeconds; }
            set {
                 showSeconds = value;
            }
        }

        public bool TopMost
        {
            get { return topMost; }
            set
            {
                topMost = value;
            }
        }

        /// <summary>Кисть для цифр циферблата</summary>
        public Brush Foreground{
            get { return new SolidColorBrush(Color.FromArgb(fgA, fgR, fgG, fgB)); } 
            set {
                Color c = ((SolidColorBrush)value).Color;

                if (c != Color.FromArgb(fgA, fgR, fgG, fgB))
                {
                    fgA = c.A;
                    fgR = c.R;
                    fgG = c.G;
                    fgB = c.B;
                }
            }
        }

        /// <summary>Background циферблата</summary>
        public Brush ClockBackground{
            get { return new SolidColorBrush(Color.FromArgb(cfA, cfR, cfG, cfB)); }
            set {
                Color c = ((SolidColorBrush)value).Color;

                if (c != Color.FromArgb(cfA, cfR, cfG, cfB))
                {
                    cfA = c.A;
                    cfR = c.R;
                    cfG = c.G;
                    cfB = c.B;
                }
            }
        }

        public bool Autoload
        {
            get { return autoload; }
            set {
                 autoload = value;
            }
        }
#endregion settings
        public bool Save()
        {
            System.IO.BinaryWriter bw = null;
            System.IO.FileStream fs = null;
            string path = null;

            try
            {    
#if DEBUG
                path = "settings.dat";
#else
                path = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Clocks");

                if (!System.IO.Directory.Exists(path))
                    System.IO.Directory.CreateDirectory(path);

                path = System.IO.Path.Combine(path, "settings.dat");
#endif
                fs = new System.IO.FileStream(
                    path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.Read);
                bw = new System.IO.BinaryWriter(fs);

                bw.Write(cfA);
                bw.Write(cfR);
                bw.Write(cfG);
                bw.Write(cfB);

                bw.Write(fgA);
                bw.Write(fgR);
                bw.Write(fgG);
                bw.Write(fgB);

                bw.Write(autoload);
                bw.Write(showSeconds);

                bw.Write(x);
                bw.Write(y);
                bw.Write(topMost);

                bw.Flush();
            }
            catch(Exception ex)
            {
#if DEBUG
//                System.Windows.MessageBox.Show("path "+path+"\nerr "+ ex.Message);
#endif
                return false;
            }
            finally
            {
                if (bw != null) bw.Dispose();
            }

            return true;
        }

        public void Load(AsyncCallback callback)
        {
            Func<bool> ac = () =>
            {
                string path;
#if DEBUG
                path = "settings.dat";
#else
                path = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "Clocks");

                path = System.IO.Path.Combine(path, "settings.dat");
#endif
                if (System.IO.File.Exists(path))
                {
                    System.IO.BinaryReader br = null;
                    System.IO.FileStream fs = null;
                    bool needDispose = true;

                    try
                    {
                        fs = new System.IO.FileStream(
                                path, System.IO.FileMode.Open,
                                System.IO.FileAccess.Read, System.IO.FileShare.Read);

                        br = new System.IO.BinaryReader(fs);

                        cfA = br.ReadByte();
                        cfR = br.ReadByte();
                        cfG = br.ReadByte();
                        cfB = br.ReadByte();

                        fgA = br.ReadByte();
                        fgR = br.ReadByte();
                        fgG = br.ReadByte();
                        fgB = br.ReadByte();

                        autoload = br.ReadBoolean();
                        showSeconds = br.ReadBoolean();

                        x = br.ReadDouble();
                        y = br.ReadDouble();
                        topMost = br.ReadBoolean();
                    }
                    catch (System.IO.EndOfStreamException ex)
                    {
#if DEBUG
                //        System.Windows.MessageBox.Show("1 path " + path + "\n" + ex.Message);
#endif
                        //освобождаем поток, чтоб удалить файл
                        if (br != null) br.Dispose();

                        needDispose = false;

                        try
                        {
                            System.IO.File.Delete(path);
                        }
                        catch (UnauthorizedAccessException) { }

                        return false;
                    }
                    catch(Exception ex)
                    {
#if DEBUG
                     //   System.Windows.MessageBox.Show("2 path " + path + "\n" +ex.Message);
#endif
                        return false;
                    }
                    finally
                    {
                        if (needDispose)
                        {
                            if (br != null)
                                br.Dispose();
                        }
                    }
                }
                else
                {
                    fgA = 255; fgR = 95; fgG = 158; fgB = 160;
                    cfA = 255; cfR = 173; cfG = 216; cfB = 230;
                }

                return true;
            };

            ac.BeginInvoke(callback, null);
        }
    }
}