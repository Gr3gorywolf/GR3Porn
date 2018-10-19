using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using System.Collections.Generic;
using xvideos_downloader;
using System.Threading;
using App1;
using System;
using Android.Content;
using System.IO;
using Android.Views;
using System.Net;

namespace GR3porno
{
    [Activity(Label = "@string/app_name", Theme = "@style/Theme.DesignDemo", MainLauncher = true, ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
    public class MainActivity : AppCompatActivity
    {
#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
        public ProgressDialog dialogoprogreso;
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos

        public Modals.pagedata listaa = new Modals.pagedata();

        public xvideossuperscraper scraper = new xvideossuperscraper();
        public ListView listilla;
        public ImageView anterior;
        public ImageView siguiente;
        public TextView textoselec;
        public Android.Support.V7.Widget.SearchView searchview;
        int pos = 0;
        int paginaact = 0;
        public int maxpages = 0;
        string termino = "";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            List<string> arraydatos = new List<string>();
            arraydatos.Add(Android.Manifest.Permission.ReadExternalStorage);
            arraydatos.Add(Android.Manifest.Permission.WriteExternalStorage);
            if (Build.VERSION.SdkInt >= BuildVersionCodes.M)
            {
                RunOnUiThread(() =>
                {
                    RequestPermissions(arraydatos.ToArray(), 0);
                });
            }

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            listilla = FindViewById<ListView>(Resource.Id.listview1);
            listaa.videos = new List<Modals.videosmodels>();
            anterior = FindViewById<ImageView>(Resource.Id.btatras);
            siguiente = FindViewById<ImageView>(Resource.Id.btdelante);
            textoselec = FindViewById<TextView>(Resource.Id.nopaginas);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.my_toolbar);
            SetSupportActionBar(toolbar);
            searchview = FindViewById<Android.Support.V7.Widget.SearchView>(Resource.Id.searchView);
            SupportActionBar.Title = "GR3Porno";
            textoselec.Click += delegate
            {

                NumberPicker selector = new NumberPicker(this);
                selector.MinValue = 0;
                selector.MaxValue = maxpages;
                selector.Value = paginaact;
                var alerta = new Android.App.AlertDialog.Builder(this)
                 .SetView(selector).
                 SetTitle("Seleccione la pagina")
                 .SetPositiveButton("Ir", (s, a) =>
                 {
                     new Thread(() =>
                     {
                         buscardatos(1, termino.Trim(), selector.Value);
                     }).Start();
                 })
                 .SetCancelable(true)

                 .SetNegativeButton("Cancelar", (s, a) =>
                   {

                   }).Show();

            };

            searchview.QueryTextSubmit += delegate
            {
                SupportActionBar.SetHomeAsUpIndicator(Resource.Drawable.home);
                SupportActionBar.SetHomeButtonEnabled(true);
                SupportActionBar.SetDisplayHomeAsUpEnabled(true);
                new Thread(() =>
                {
                   

                    try
                    {
                      
                        buscardatos(1, searchview.Query, 0);
                        termino = searchview.Query;
                    }
                    catch (Exception e)
                    {

                        RunOnUiThread(() =>
                        {

                            var msg = e;
                            dialogoprogreso.Dismiss();
                            Toast.MakeText(this, "No se han encontrado resultados de busqueda", ToastLength.Long).Show();
                            Console.WriteLine(e.Message);
                        });
                    }

                }).Start();

            };

            setdialog("Conectandose al servidor", "Por favor espere...");
            new Thread(() =>
            {



            if (CheckInternetConnection())
            {
                    RunOnUiThread(() => { dialogoprogreso.Dismiss(); });
                buscardatos(1, "", 0);


            }
            else
            {
                    RunOnUiThread(() =>
                    {
                        dialogoprogreso.Dismiss();
                        new Android.App.AlertDialog.Builder(this)
                      .SetTitle("No se ha podido conectar al servidor")
                      .SetMessage("Por favor revise su conexion a internet")
                      .SetCancelable(true)


                      .SetPositiveButton("Ok", (aax, az) =>
                      {
                          this.Finish();
                      })
                      .Create().Show();
                    });

                }
            }).Start();
         
            anterior.Click += delegate
            {
                new Thread(() =>
                {
                    buscardatos(1, termino.Trim(), paginaact - 1);
                }).Start();

            };
            siguiente.Click += delegate
            {
                new Thread(() =>
                {
                    buscardatos(1, termino.Trim(), paginaact + 1);
                }).Start();


            };
            listilla.ItemClick += (ax, fr) =>
            {
                pos = fr.Position;
                new Android.App.AlertDialog.Builder(this)
                .SetTitle("Que desea hacer con este video?")
                .SetCancelable(true)

                .SetNegativeButton("Ver online", ver)
                .SetPositiveButton("Descargar", descargar)
                .Create().Show();
            };
            base.OnCreate(savedInstanceState);

        }


        public void ver(object sender, EventArgs e)
        {


            new Thread(() =>
            {

                RunOnUiThread(() =>
                {
                    setdialog("Obteniendo link de streaming", "Por favor espere...");
                });
                var link = scraper.getdownloadlink(listaa.videos[pos].link, 1);
                RunOnUiThread(() =>
                {
                    dialogoprogreso.Dismiss();
                });
                Intent intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(Android.Net.Uri.Parse(link), "video/*");
                StartActivity(intent);


            }).Start();

        }
        public void descargar(object sender, EventArgs e)
        {




            new Thread(() =>
            {
                RunOnUiThread(() =>
                {
                    setdialog("Obteniendo link de descarga", "Por favor espere...");
                });
                var link = scraper.getdownloadlink(listaa.videos[pos].link, 1).Replace(" ", "%20");
                var manige = DownloadManager.FromContext(this);
                var requ = new DownloadManager.Request(Android.Net.Uri.Parse(link));
                requ.SetDescription("Espere por favor");
                requ.SetNotificationVisibility(DownloadVisibility.VisibleNotifyCompleted);
                requ.SetTitle(listaa.videos[pos].title + ".mp4");
                string downloadpath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDownloads);
                Android.Net.Uri destino = null;
                destino = Android.Net.Uri.FromFile(new Java.IO.File(downloadpath + "/" + listaa.videos[pos].title + ".mp4"));
                requ.SetDestinationUri(destino);
                requ.SetVisibleInDownloadsUi(true);
                manige.Enqueue(requ);

                RunOnUiThread(() => {
                    dialogoprogreso.Dismiss();
                    Toast.MakeText(this, "Descarga iniciada!!", ToastLength.Long).Show();

                });

            }).Start();



        }


      

        public override void OnBackPressed()
        {

            if (termino != "")
            {
                new Thread(() =>
                {
                    buscardatos(1, "", 0);
                    termino = "";
                    RunOnUiThread(() =>
                    {
                        SupportActionBar.SetDisplayHomeAsUpEnabled(false);
                        searchview.SetQuery("", false);
                    });
                }).Start();

            }
            else {
                base.OnBackPressed();
            }
          
        

        }
        public void setdialog(string titulo, string mensaje)
        {
            RunOnUiThread(() =>
            {

#pragma warning disable CS0618 // El tipo o el miembro están obsoletos
                dialogoprogreso = new ProgressDialog(this);
#pragma warning restore CS0618 // El tipo o el miembro están obsoletos

                dialogoprogreso.SetCanceledOnTouchOutside(false);
                dialogoprogreso.SetCancelable(false);
                dialogoprogreso.SetTitle(titulo);
                dialogoprogreso.SetMessage(mensaje);
                dialogoprogreso.Show();
            });
        }
        public void buscardatos(int paginas, string querry, int pagina) {

            paginaact = pagina;
            RunOnUiThread(() =>
            {
                setdialog("Buscando pornos", "Por favor espere...");
            });
            listaa.videos.Clear();
       
                listaa = scraper.getvideos(paginas, querry, pagina);

                Android.Views.ViewStates estado = Android.Views.ViewStates.Visible;
                Android.Views.ViewStates estado2 = Android.Views.ViewStates.Visible;
                if (listaa.navigationmax <= paginaact)
                    estado = Android.Views.ViewStates.Invisible;
                else
                    estado = Android.Views.ViewStates.Visible;

                if (paginaact == 0)
                    estado2 = Android.Views.ViewStates.Invisible;
                else
                    estado2 = Android.Views.ViewStates.Visible;

                RunOnUiThread(() =>
                    {
                        siguiente.Visibility = estado;
                        anterior.Visibility = estado2;
                        maxpages = listaa.navigationmax;
                        textoselec.Text = "Pagina " + paginaact + " de " + listaa.navigationmax;
                        listilla.Adapter = new adapterlistaremoto(this, listaa.videos);
                        dialogoprogreso.Dismiss();
                        try { dialogoprogreso.Dismiss(); } catch (Exception) { }
                    });
          



        }

        public bool CheckInternetConnection()
        {
            string CheckUrl = "http://www.xvideos.com/";

            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(CheckUrl);

                iNetRequest.Timeout = 27000;

                WebResponse iNetResponse = iNetRequest.GetResponse();

                // Console.WriteLine ("...connection established..." + iNetRequest.ToString ());
                iNetResponse.Close();

                return true;

            }
            catch (WebException)
            {

                // Console.WriteLine (".....no connection..." + ex.ToString ());

                return false;
            }
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    new Thread(() =>
                    {
                        buscardatos(1, "", 0);
                        termino = "";
                        RunOnUiThread(() => {
                            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
                            searchview.SetQuery("", false);
                        });
                    }).Start();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

    }

    }


