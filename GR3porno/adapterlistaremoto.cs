using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Support.V7.App;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Android.Graphics;
using System.Net;
using System.Threading;
//using Square.Picasso;
using Android.Graphics.Drawables;
using Android.Glide;
using Android.Glide.Request;
using xvideos_downloader;
using GR3porno;

namespace App1
{
    public class adapterlistaremoto : BaseAdapter
    {

        Context context;
       public List< Modals.videosmodels> elementos;
        // int pos = 0;

        public void animar(Java.Lang.Object imagen)
        {

            Android.Animation.ObjectAnimator animacion = Android.Animation.ObjectAnimator.OfFloat(imagen, "scaleX", 0.5f, 1f);
            animacion.SetDuration(300);
            animacion.Start();



        }
        public adapterlistaremoto(Context context, List< Modals.videosmodels> elements )
        {
            //List<Android.Graphics.Bitmap> imageneses

            this.context = context;
            elementos = elements;
            try {
                Glide.Get(context).ClearMemory();
            }
            catch (Exception) { 
                }
          
          //  imagenes = imageneses;

        }
        protected override void Dispose(bool disposing)
        {

           
            base.Dispose(disposing);
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            adaptadorlistaremotoViewHolder holder = null;

            if (view != null)
                holder = view.Tag as adaptadorlistaremotoViewHolder;

            if (holder == null)
            {
                holder = new adaptadorlistaremotoViewHolder();
                var inflater = context.GetSystemService(Context.LayoutInflaterService).JavaCast<LayoutInflater>();
                //replace with your item and your holder items
                //comment back in
               view = inflater.Inflate(Resource.Layout.layoutbuscadorconcarta, parent, false);
               holder.Title = view.FindViewById<TextView>(Resource.Id.textView);
                holder.Title2 = view.FindViewById<TextView>(Resource.Id.textView2);
                holder.portrait = view.FindViewById<ImageView>(Resource.Id.imageView);

              //  view.SetBackgroundResource(Resource.Drawable.drwaablegris);
               view.Tag = holder;
                /*   if (links.Contains(""))
                   {
                       links.Remove("");
                   }*/

                try
                {
                    Glide.With(context)
                  .Load(elementos[position].thumb)
                   .Apply(RequestOptions.NoTransformation().Placeholder(Resource.Drawable.nude))
                   .Into(holder.portrait);
                holder.portrait.SetTag(Resource.Id.imageView, elementos[position].thumb);
            }
                catch (Exception) { }
        }
             




            

            
            if (holder.portrait.GetTag(Resource.Id.imageView).ToString() != elementos[position].thumb)
            {
                try { 
                Glide.With(context)
              .Load(elementos[position].thumb)
              .Apply(RequestOptions.NoTransformation().Placeholder(Resource.Drawable.nude))
               .Into(holder.portrait);
                }
                catch(Exception) { }

            }




            holder.Title2.Text = elementos[position].duration;
            holder.Title.Text = elementos[position].title;
            
            holder.animar3(view);
            holder.portrait.SetTag(Resource.Id.imageView, elementos[position].thumb);


            //fill in your items
            //holder.Title.Text = "new text here";


           /// clasesettings.recogerbasura();
            return view;
        }

       
        public override int Count
        {
            get
            {
                return elementos.Count;
            }
        }

    }
   
    class adaptadorlistaremotoViewHolder : Java.Lang.Object
    {
        public TextView Title { get; set; }
        public TextView Title2 { get; set; }
        public ImageView portrait { get; set; }

        public void animar3(View imagen)
        {
            imagen.SetLayerType(LayerType.Hardware, null);
            Android.Animation.ObjectAnimator animacion = Android.Animation.ObjectAnimator.OfFloat(imagen, "scaleX", 0f, 1f);
            animacion.SetDuration(250);
            animacion.Start();
            animacion.AnimationEnd += delegate
            {
                imagen.SetLayerType(LayerType.None, null);
            };
            imagen.SetLayerType(LayerType.Hardware, null);
            Android.Animation.ObjectAnimator animacion2 = Android.Animation.ObjectAnimator.OfFloat(imagen, "scaleY", 0f, 1f);
            animacion2.SetDuration(250);
            animacion2.Start();
            animacion2.AnimationEnd += delegate
            {
                imagen.SetLayerType(LayerType.None, null);
            };
        }
        //Your adapter views to re-use
        //public TextView Title { get; set; }
    }
}