using System;
using System.IO;
using System.Text;

namespace SketchUp
{
	public class AdminData
	{
		public event EventHandler<RateChangedEventArgs> RateChangedEvent;

		public string Loc_Locality
		{
			get; set;
		}

		public string Loc_State
		{
			get; set;
		}

		public string Loc_DataDrive
		{
			get; set;
		}

		public string Loc_CamFolder
		{
			get; set;
		}

		public string Loc_CamPreFix
		{
			get; set;
		}

		public string Loc_UserId
		{
			get; set;
		}

		public string Loc_UserInit
		{
			get; set;
		}

		public string Loc_GisPath
		{
			get; set;
		}

		public string Loc_GisLayer
		{
			get; set;
		}

		public string Loc_GisField
		{
			get; set;
		}

		public string Loc_CamField
		{
			get; set;
		}

		public string Loc_SidLayer
		{
			get; set;
		}

		public string orig_Loc_Locality
		{
			get; set;
		}

		public string orig_Loc_State
		{
			get; set;
		}

		public string orig_Loc_DataDrive
		{
			get; set;
		}

		public string orig_Loc_CamFolder
		{
			get; set;
		}

		public string orig_Loc_CamPreFix
		{
			get; set;
		}

		public string orig_Loc_UserId
		{
			get; set;
		}

		public string orig_Loc_UserInit
		{
			get; set;
		}

		public string orig_Loc_GisPath
		{
			get; set;
		}

		public string orig_Loc_GisLayer
		{
			get; set;
		}

		public string orig_Loc_GisField
		{
			get; set;
		}

		public string orig_Loc_CamField
		{
			get; set;
		}

		public string orig_Loc_SidLayer
		{
			get; set;
		}

		public static string camDataDrive = String.Empty;
		public static string camFolder = String.Empty;
		public static string camLib = String.Empty;

		public bool AdminDataIsChanged
		{
			get
			{
				camDataDrive = Loc_DataDrive;
				camFolder = Loc_CamFolder;
				camLib = Loc_CamPreFix;
				return (orig_Loc_Locality != Loc_Locality
					|| orig_Loc_State != Loc_State
					|| orig_Loc_DataDrive != Loc_DataDrive
					|| orig_Loc_CamFolder != Loc_CamFolder
					|| orig_Loc_CamPreFix != Loc_CamPreFix
					|| orig_Loc_UserId != Loc_UserId
					|| orig_Loc_UserInit != Loc_UserInit
					|| orig_Loc_GisPath != Loc_GisPath
					|| orig_Loc_GisLayer != Loc_GisLayer
					|| orig_Loc_GisField != Loc_GisField
					|| orig_Loc_CamField != Loc_CamField
					|| orig_Loc_SidLayer != Loc_SidLayer
					);
			}
		}

		private bool _isDirtyCheckingOn = false;

		private void FireChangedEvent(string property)
		{
			if (_isDirtyCheckingOn)
			{
				if (RateChangedEvent != null)
				{
					RateChangedEvent(this,
						new RateChangedEventArgs()
						{
							PropertyName = property
						});
				}
			}
		}

		public class RateChangedEventArgs : EventArgs
		{
			public RateChangedEventArgs()
				: base()
			{
			}

			public string PropertyName
			{
				get; set;
			}
		}
	}
}