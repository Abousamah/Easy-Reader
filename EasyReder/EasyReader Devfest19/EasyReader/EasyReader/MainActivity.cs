using Android.App;
using Android.Widget;
using Android.OS;
using System.Text;
using Android.Gms.Vision.Texts;
using Android.Util;
using Android.Gms.Vision;
using Android.Graphics;
using System.IO;
using Android.Support.V7.App;
using Android.Content;
using Android.Runtime;
using Android.Provider;


namespace EasyReader
{
    [Activity(Label = "Easy Reader", MainLauncher = true)]
    public class MainActivity : Activity
	{
        private ImageView imageview;
        private Button btnProcess;
        private TextView txtView;
		Bitmap bitmap;

		protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            imageview = FindViewById<ImageView>(Resource.Id.image_view);
            btnProcess = FindViewById<Button>(Resource.Id.btnProcess);
            txtView = FindViewById<TextView>(Resource.Id.txtView);

			Bitmap bitmap = BitmapFactory.DecodeResource(ApplicationContext.Resources, Resource.Drawable.Icon);
			imageview.SetImageBitmap(bitmap);
			btnProcess.Click += BtnProcess_Click;


			//btnProcess.Click += delegate
   //         {
   //             TextRecognizer txtRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();
   //             if (!txtRecognizer.IsOperational)
   //             {
   //                 Log.Error("Error", "Detector dependencies are not yet available");
   //             }
   //             else
   //             {
   //                 Frame frame = new Frame.Builder().SetBitmap(bitmap).Build();
   //                 SparseArray items = txtRecognizer.Detect(frame);
   //                 StringBuilder strBuilder = new StringBuilder();
   //                 for (int i = 0; i < items.Size(); i++)
   //                 {
   //                     TextBlock item = (TextBlock)items.ValueAt(i);
   //                     strBuilder.Append(item.Value);
   //                     strBuilder.Append("/");
   //                 }
   //                 txtView.Text = strBuilder.ToString();
   //             }
   //         };


			Intent intent = new Intent(MediaStore.ActionImageCapture);
			StartActivityForResult(intent, 0);
		}

		private void BtnProcess_Click(object sender, System.EventArgs e)
		{
			TextRecognizer txtRecognizer = new TextRecognizer.Builder(ApplicationContext).Build();
			if (!txtRecognizer.IsOperational)
			{
				Log.Error("Error", "Detector dependencies are not yet available");
			}
			else
			{
				Frame frame = new Frame.Builder().SetBitmap(bitmap).Build();
				SparseArray items = txtRecognizer.Detect(frame);
				StringBuilder strBuilder = new StringBuilder();
				for (int i = 0; i < items.Size(); i++)
				{
					TextBlock item = (TextBlock)items.ValueAt(i);
					strBuilder.Append(item.Value);
					strBuilder.Append("/");
				}
				txtView.Text = strBuilder.ToString();
			}
		}

		protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);
			 bitmap = (Bitmap)data.Extras.Get("data");
			imageview.SetImageBitmap(bitmap);

			//Bitmap bitmap = BitmapFactory.DecodeResource(ApplicationContext.Resources, Resource.Drawable.ahsan);
			//imageview.SetImageBitmap(bitmap);

			//var client = ImageAnnotatorClient.Create();
			////var image = Image.FromUri("gs://cloud-vision-codelab/otter_crossing.jpg");
			//var path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
			//Bitmap bm = BitmapFactory.DecodeFile(path);
			//MemoryStream stream = new MemoryStream();
			//bm.Compress(Bitmap.CompressFormat.Jpeg, 100, stream);
			//Google.Cloud.Vision.V1.Image img = Google.Cloud.Vision.V1.Image.FromStream(stream);



			//var response = client.DetectText(img);
			//foreach (var annotation in response)
			//{
			//	if (annotation.Description != null)
			//	{
			//		//Console.WriteLine(annotation.Description);
			//		Toast.MakeText(this, annotation.Description, ToastLength.Short).Show();
			//	}
			//}


		}

		public static byte[] ReadFully(Stream input)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				input.CopyTo(ms);
				return ms.ToArray();
			}
		}
		public static byte[] StreamToByteArray(Stream stream)
		{
			if (stream is MemoryStream)
			{
				return ((MemoryStream)stream).ToArray();
			}
			else
			{
				// Jon Skeet's accepted answer 
				return ReadFully(stream);
			}
		}
	}
}

