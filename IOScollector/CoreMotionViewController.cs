using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;

using System;
using CoreGraphics;

using Foundation;
using UIKit;
using CoreMotion;

namespace CoreMotion
{
	public partial class CoreMotionViewController : UIViewController
	{
		private CMMotionManager motionManager;

		public CoreMotionViewController () : base ("CoreMotionViewController", null)
		{
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			motionManager = new CMMotionManager ();

			motionManager.StartAccelerometerUpdates (NSOperationQueue.CurrentQueue, (data, error) => 
				{
					this.lblX.Text = data.Acceleration.X.ToString ("0.00000000");
					this.lblY.Text = data.Acceleration.Y.ToString ("0.00000000");
					this.lblZ.Text = data.Acceleration.Z.ToString ("0.00000000");
				});

		}

		public override void ViewDidUnload ()
		{
			base.ViewDidUnload ();

			ReleaseDesignerOutlets ();
		}

		public override bool ShouldAutorotateToInterfaceOrientation (UIInterfaceOrientation toInterfaceOrientation)
		{
			return (toInterfaceOrientation != UIInterfaceOrientation.PortraitUpsideDown);
		}



		partial void UIButton21_TouchUpInside (UIButton sender)
		{
			throw new NotImplementedException ();
		}

		partial void UIButton25_TouchUpInside (UIButton sender)
		{
			throw new NotImplementedException ();
		} 

		// to perform container operations on Microsft Azure Blob Storage 
		public class MainActivity : Activity
		{
			int count = 1;
			string sas = "https://ucltt.blob.core.windows.net/collector/?sv=2015-04-05&sr=c&sig=E3KK%2BaWJVw8vemkDM8%2BsV9n7K5SLdgstXX1RuSTvBsc%3D&st=2015-12-14T11%3A30%3A06Z&se=2016-06-14T10%3A30%3A06Z&sp=rwdl&comp=list&restype=container";
			protected override void OnCreate(Bundle bundle)
			{
				base.OnCreate(bundle);

				// Set the view from the "main" layout resource
				SetContentView(Resource.Layout.Main);

				// Get the button from the layout resource, and attach an event to it
				UIButton = FindViewById<Button>(Resource.Id.StartButton);

				button.Click += async delegate  {
					button.Text = string.Format("{0} clicks!", count++);
					await UseContainerSAS(sas);
				};

			}
//		UIButton25_TouchUpInside = false;
//		UIButton21_TouchUpInside.click += (object sender, EventArgs e) =>
	//	{
	//		starttime = DateTime.Now;
	//		UIButton21_TouchUpInside = false;
	//		UIButton25_TouchUpInside = true;
	//	}
	}
}

	static async _Task UseContainerSAS(string sas)
	{
		//Try performing container operations with the SAS provided.

		//Return a reference to the container using the SAS URI.
		CloudBlobContainer container = new CloudBlobContainer(new Uri(sas));
		string date = DateTime.Now.ToString();
		try
		{
			//Write operation: write a new blob to the container.
			CloudBlockBlob blob = container.GetBlockBlobReference("sasblob_" + date + ".txt");

			string blobContent = "This blob was created with a shared access signature granting write permissions to the container. ";
			MemoryStream msWrite = new
				MemoryStream(Encoding.UTF8.GetBytes(blobContent));
			msWrite.Position = 0;
			using (msWrite)
			{
				await blob.UploadFromStreamAsync(msWrite);
			}
			Console.WriteLine("Write operation succeeded for SAS " + sas);
			Console.WriteLine();
		}
		catch (Exception e)
		{
			Console.WriteLine("Write operation failed for SAS " + sas);
			Console.WriteLine("Additional error information: " + e.Message);
			Console.WriteLine();
		}
		try
		{
			//Read operation: Get a reference to one of the blobs in the container and read it.
			CloudBlockBlob blob = container.GetBlockBlobReference("sasblob_” + date + “.txt");
			string data = await blob.DownloadTextAsync();

			Console.WriteLine("Read operation succeeded for SAS " + sas);
			Console.WriteLine("Blob contents: " + data);
		}
		catch (Exception e)
		{
			Console.WriteLine("Additional error information: " + e.Message);
			Console.WriteLine("Read operation failed for SAS " + sas);
			Console.WriteLine();
		}
		Console.WriteLine();
		try
		{
			//Delete operation: Delete a blob in the container.
			CloudBlockBlob blob = container.GetBlockBlobReference("sasblob_” + date + “.txt");
			await blob.DeleteAsync();

			Console.WriteLine("Delete operation succeeded for SAS " + sas);
			Console.WriteLine();
		}
		catch (Exception e)
		{
			Console.WriteLine("Delete operation failed for SAS " + sas);
			Console.WriteLine("Additional error information: " + e.Message);
			Console.WriteLine();
		}
	}