using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace JioSavaanVideo
{
    class Program
    {
        static void Main(string[] args)
        {
            try

            {

                string[] foldercreate = { "JioSaavn" };
                string foldername = string.Empty;
                var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + @"E:\JioSavaanVideo\JioSavaanVideo.xlsx" + ";Extended Properties=Excel 12.0;");

                OleDbConnection connexcel = new OleDbConnection(connectionString);
                connexcel.Open();
                DataTable sheets = connexcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                String[] excelSheetNames = new String[sheets.Rows.Count];
                int sn = 0;
                int VideoCount = 0;
                excelSheetNames[0] = "Sheet1$";
                var adapterbvandrev1 = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                // Start distinct upc code
                var upc_distnct = new OleDbDataAdapter("SELECT DISTINCT ImgID FROM [Sheet1$]", connectionString);
                var ds = new DataSet();
                upc_distnct.Fill(ds);
                DataTable dt = ds.Tables[0];
                int CountRow = dt.Rows.Count;
                string[] upc = new string[dt.Rows.Count];
                string FileName = "";
                string bitrate = string.Empty;
                // Loop for different Upc Or i can say No. of diff albums
                string SSSS = "";
                for (int i = 0; i < CountRow; i++)
                {
                    int uppc = i;
                    upc[i] = dt.Rows[i]["ImgID"].ToString();
                    SSSS = SSSS + "," + dt.Rows[i]["ImgID"].ToString();
                }
                string previousvalue = string.Empty;
                foreach (var item in foldercreate)
                {

                    for (int j = 0; j < CountRow; j++)
                    {
                        int a = 0;
                        foldername = DateTime.Now.ToString("yyyyMMddHHmmss") + "000";
                        FileName = upc[j].ToString();
                        //create directory
                        System.IO.Directory.CreateDirectory(@"E:\JioSavaanVideo\" + item + "\\" + foldername + "\\" + FileName);
                        System.IO.Directory.CreateDirectory(@"E:\JioSavaanVideo\" + item + "\\" + foldername + "\\" + FileName + "\\resources");
                        int tracksdds = j;
                        var UpcTracks = new OleDbDataAdapter("SELECT *  FROM [Sheet1$] WHERE ImgID=" + "'" + FileName + "'", connectionString);
                        var UpcTrackds = new DataSet();
                        UpcTracks.Fill(UpcTrackds);
                        DataTable Upcdt = UpcTrackds.Tables[0];
                        int TrackCount = Upcdt.Rows.Count;
                        XElement guvera = new XElement("MessageHeader"


                            , new XElement("MessageThreadId", foldername)
                            //, new XElement("MessageId", foldername)
                          //
                          , new XElement("MessageId", "15a4cc6f160a45babb9f9bdcbc253d42")
                          , new XElement("MessageSender", new XElement("PartyId", "PADPIDA20191001061")
                          , new XElement("PartyName", new XElement("FullName", (item == "Spotify" ? "Seven Colors" : "Unisys"))))
                          , new XElement("MessageRecipient"
                          , new XElement("PartyId", "PADPIDA2012073007R")
                          //, new XElement("PartyId", "PADPIDA2018010804X")
                          , new XElement("PartyName", new XElement("FullName", "Saavn")))
                          // , new XElement("MessageRecipient", new XElement("PartyId", "PADPIDA2015120100H")
                          //, new XElement("PartyName", new XElement("FullName", "OriginalMessage")))

                          //DateTime.UtcNow.ToString("s") , new XElement("MessageCreatedDateTime", (DateTime.Now.Date).ToString("yyyy - MM - dd").Replace(" ", "") + "T" + DateTime.Now.ToString("HH:mm:ss")));
                          , new XElement("MessageCreatedDateTime", DateTime.UtcNow.ToString("s") + "+05:30")
                          , new XElement("MessageControlType", "LiveMessage")


                          );
                        XElement MessageHeaderSaavn = new XElement(new XElement("MessageControlType", "LiveMessage"));
                      
                 //       XElement UpdateNode = new XElement("UpdateIndicator", "OriginalMessage");
                        // Resource List strats here
                        XElement Resource = new XElement("ResourceList");
                        string sourceFile = string.Empty;
                        string destFile = string.Empty;
                        string UpdatedJpgHashSUm = string.Empty;
                        for (int l = 0; l < TrackCount; l++)
                        {
                            string hashsumdestFile = string.Empty;
                            string updateHashSum = string.Empty;
                            UpdatedJpgHashSUm = string.Empty;
                            FileInfo fi = new FileInfo(Upcdt.Rows[l]["TrackPath"].ToString());
                            string ext = fi.Extension;
                            if (ext.ToLower() == ".mp4" || ext.ToLower() == ".mov")
                            {
                                sourceFile = System.IO.Path.Combine(Upcdt.Rows[l]["TrackPath"].ToString());

                                //var videoFrameReader = new VideoFrameReader(sourceFile);
                                //bitrate = videoFrameReader.BitRate.ToString();
                                hashsumdestFile = System.IO.Path.Combine(@"E:\JioSavaanVideo\" + item + "\\" + foldername + "\\" + FileName + "\\resources", fi.Name);
                                updateHashSum = @"E:\JioSavaanVideo\" + item + "\\" + foldername + "\\" + FileName + "\\resources\\" + Upcdt.Rows[l]["FileName"].ToString() + ".mp4";
                                System.IO.File.Copy(sourceFile, hashsumdestFile, true);
                                System.IO.File.Move(hashsumdestFile, updateHashSum);

                            }
                            FileInfo fiimg = new FileInfo(Upcdt.Rows[l]["ImagePath"].ToString());
                            string extimg = fiimg.Extension;
                            UpdatedJpgHashSUm = @"E:\JioSavaanVideo\" + item + "\\" + foldername + "\\" + FileName + "\\resources\\" + Upcdt.Rows[l]["ImgID"].ToString() + ".jpg";
                            if (l == 0)
                            {
                                if (extimg.ToLower() == ".jpg")
                                {
                                    sourceFile = string.Empty;
                                    destFile = string.Empty;
                                    sourceFile = System.IO.Path.Combine(Upcdt.Rows[l]["ImagePath"].ToString());

                                    destFile = System.IO.Path.Combine(@"E:\JioSavaanVideo\" + item + "\\" + foldername + "\\" + FileName + "\\resources", fiimg.Name);

                                    System.IO.File.Copy(sourceFile, destFile, true);
                                    System.IO.File.Move(destFile, UpdatedJpgHashSUm);
                                }
                            }

                            string gsr = Upcdt.Rows[l]["GGL_USR_ID"].ToString().Substring((Upcdt.Rows[l]["GGL_USR_ID"].ToString().Length - 4), 4);
                            if (gsr == previousvalue)
                            {
                                a = a;
                            }
                            else
                            {
                                a++;
                            }
                            XElement SoundRecording = new XElement("Video", new XElement("VideoType", "ShortFormMusicalWorkVideo")
                           , new XElement("VideoId", new XElement("ISRC", Upcdt.Rows[l]["ISRC_Video"])
                           //, new XElement("CatalogNumber", new XAttribute("Namespace", "DPID:"), Upcdt.Rows[l]["GGL_USR_ID"].ToString().Substring((Upcdt.Rows[l]["GGL_USR_ID"].ToString().Length - 4), 4) + "_0"+ a)
                           //, new XElement("ProprietaryId", new XAttribute("Namespace", "DPID:"), Upcdt.Rows[l]["GGL_USR_ID"] + "_0" + a)
                           )
                           , new XElement("ResourceReference", "A" + (l + 1))
                           , new XElement("ReferenceTitle", new XElement("TitleText", Upcdt.Rows[l]["RTitleText"]))
                           , new XElement("LanguageOfPerformance", Upcdt.Rows[l]["LOP"])
                           , new XElement("Duration", "PT" + (DateTime.Parse(Upcdt.Rows[l]["Duration"].ToString()).Hour).ToString() + "H" + (DateTime.Parse(Upcdt.Rows[l]["Duration"].ToString()).ToString("mm")) + "M" + (DateTime.Parse(Upcdt.Rows[l]["Duration"].ToString()).ToString("ss")) + "S")
                           , new XElement("VideoDetailsByTerritory", new XElement("TerritoryCode", Upcdt.Rows[l]["TerritoryCode"])
                           , new XElement("Title", new XAttribute("TitleType", "FormalTitle"), new XElement("TitleText", Upcdt.Rows[l]["TTitleText"])
                           //, new XElement("SubTitle", "Live")
                           )
                           , new XElement("Title", new XAttribute("TitleType", "DisplayTitle"), new XElement("TitleText", Upcdt.Rows[l]["TTitleText"]))
                           , getDisplayAttrvalue(Upcdt.Rows[l]["FullName"].ToString(), "FullName")
                           , getDisplayAttrvalue(Upcdt.Rows[l]["FeaturedArtist"].ToString(), "FeaturedArtist")
                           , getAttrvalue(Upcdt.Rows[l]["RCProducer"].ToString(), "Producer", "ResourceContributor", "ResourceContributorRole")
                           , getAttrvalue(Upcdt.Rows[l]["Composer"].ToString(), "Composer", "IndirectResourceContributor", "IndirectResourceContributorRole")
                           //, new XElement("LabelName", Upcdt.Rows[l]["LabelName"])
                           , new XElement("ResourceReleaseDate", Convert.ToDateTime(Upcdt.Rows[l]["OriginalReleaseDate"]).ToString("yyyy-MM-dd"))
                           //, new XElement("RightsController", new XElement("PartyName", new XElement("FullName", Upcdt.Rows[l]["LabelName"]))
                           //, new XElement("RightsControllerRole", "RightsController")
                           //, new XElement("RightSharePercentage", "100.0")
                           //)
                           , new XElement("PLine", new XElement("Year", Upcdt.Rows[l]["Year"]), new XElement("PLineText", Upcdt.Rows[l]["PLineText"]))
                           , new XElement("Genre", new XElement("GenreText", Upcdt.Rows[l]["GenreText"])
                           , new XElement("SubGenre", Upcdt.Rows[l]["SubGenre"]))

                           , new XElement("ParentalWarningType", "NotExplicit")
                           , new XElement("TechnicalVideoDetails", new XElement("TechnicalResourceDetailsReference", "T" + (l + 1))
                          // , new XElement("VideoCodecType", new XAttribute("Namespace", "DPID:PADPIDA20191001061"), "QuickTime")
                          // , new XElement("VideoBitRate", new XAttribute("UnitOfMeasure", "kbps"), bitrate)
                           // , new XElement("IsPreview", "false")
                          // , new XElement("FrameRate", "25")
                          // , new XElement("ImageHeight", "1080")
                          // , new XElement("ImageWidth", "1920")
                          // , new XElement("AudioBitRate", new XAttribute("UnitOfMeasure", "kbps"), "1411")
                          // , new XElement("NumberOfAudioChannels", "2")
                          // , new XElement("AudioSamplingRate", new XAttribute("UnitOfMeasure", "Hz"), "44100")
                          // , new XElement("IsPreview", "false")
                          //, new XElement("PreviewDetails", new XElement("StartPoint", "15")
                          //, new XElement("Duration", "PT" + (DateTime.Parse(Upcdt.Rows[l]["Duration"].ToString()).Hour).ToString() + "H" + (DateTime.Parse(Upcdt.Rows[l]["Duration"].ToString()).ToString("mm")) + "M" + (DateTime.Parse(Upcdt.Rows[l]["Duration"].ToString()).ToString("ss")) + "S")
                          //, new XElement("ExpressionType", "Informative"))
                           , new XElement("File", new XElement("FileName", Upcdt.Rows[l]["FileName"] + ".mp4"), new XElement("FilePath", "resources/")
                         //  , new XElement("HashSum", new XElement("HashSum", GetHashSum(updateHashSum)), new XElement("HashSumAlgorithmType", "MD5"))
                           )))

                           );
                            previousvalue = Upcdt.Rows[l]["GGL_USR_ID"].ToString().Substring((Upcdt.Rows[l]["GGL_USR_ID"].ToString().Length - 4), 4);

                            VideoCount = l;
                            Resource.Add(SoundRecording);
                        }
                        var Text = "0:0:0";
                        TimeSpan Span1 = TimeSpan.Parse(Text);
                        for (int v = 0; v < TrackCount; v++)
                        {
                            TimeSpan Span2 = TimeSpan.Parse(Upcdt.Rows[v]["Duration"].ToString());
                            Span1 = Span1 + Span2;
                        }
                        //Console.WriteLine(timestamp(Span1.ToString()));




                        // Start Image Tag============================================================
                        XElement ImageTag = new XElement("Image", new XElement("ImageType", "VideoScreenCapture"), new XElement("ImageId", new XElement("ProprietaryId", new XAttribute("Namespace", "DPID:PADPIDA20191001061"), Upcdt.Rows[0]["GGL_USR_ID"]))
                        , new XElement("ResourceReference", "A" + (VideoCount + 2))
                        , new XElement("ImageDetailsByTerritory", new XElement("TerritoryCode", Upcdt.Rows[0]["TerritoryCode"])
                        , new XElement("ParentalWarningType", Upcdt.Rows[0]["ParentalWarning"])
                        , new XElement("TechnicalImageDetails", new XElement("TechnicalResourceDetailsReference", "T" + (VideoCount + 2))
                        //, new XElement("ImageCodecType", "JPEG"), new XElement("ImageHeight", new XAttribute("UnitOfMeasure", "Pixel"), "720"), new XElement("ImageWidth", new XAttribute("UnitOfMeasure", "Pixel"), "1280")
                        , new XElement("File", new XElement("FileName", Upcdt.Rows[0]["ImgID"] + ".jpg"),new XElement("FilePath", "resources"))
                        //, new XElement("HashSum", new XElement("HashSum", GetHashSum(UpdatedJpgHashSUm)), new XElement("HashSumAlgorithmType", "MD5")))
                        )));

                        // End Image Tag==============================================================
                        // Start of Release List======================================================

                        XElement ReleaseListTag = new XElement("ReleaseList");
                        for (int p = 0; p < (TrackCount + 1); p++)

                        {
                            if (p == 0)
                            {
                                XElement ReleaseTag = new XElement("Release", new XAttribute("IsMainRelease", "true"), new XElement("ReleaseId", new XElement("ICPN", new XAttribute("IsEan", "false"), Upcdt.Rows[p]["ImgID"])
                                 //, new XElement("CatalogNumber", new XAttribute("Namespace", "DPID:"), Upcdt.Rows[p]["GGL_USR_ID"].ToString().Substring((Upcdt.Rows[p]["GGL_USR_ID"].ToString().Length - 4), 4))
                                 //, new XElement("ProprietaryId", new XAttribute("Namespace", "DPID:"), Upcdt.Rows[p]["GGL_USR_ID"])
                                 )
                             , new XElement("ReleaseReference", "R" + p), new XElement("ReferenceTitle", new XElement("TitleText", Upcdt.Rows[p]["VideoTitleText"])
                             //, new XElement("SubTitle", "Live")
                             ));
                                XElement ReleaseResourceReferenceList = new XElement("ReleaseResourceReferenceList");
                                for (int n = 1; n <= TrackCount; n++)
                                {
                                    XElement ReleaseReference = new XElement("ReleaseResourceReference", new XAttribute("ReleaseResourceType", "PrimaryResource"), "A" + n);
                                    ReleaseResourceReferenceList.Add(ReleaseReference);
                                }
                                XElement Secondary = new XElement("ReleaseResourceReference", new XAttribute("ReleaseResourceType", "SecondaryResource"), "A" + (TrackCount + 1));
                                ReleaseResourceReferenceList.Add(Secondary);
                                ReleaseTag.Add(ReleaseResourceReferenceList);
                                XElement ReleaseType = new XElement("ReleaseType", (TrackCount == 1) ? "VideoSingle" : "VideoTrackRelease");
                                ReleaseTag.Add(ReleaseType);
                                XElement ReleaseDetailsByTerritory = new XElement("ReleaseDetailsByTerritory", new XElement("TerritoryCode", Upcdt.Rows[p]["TerritoryCode"])
                                    , new XElement("DisplayArtistName", /*new XAttribute("SequenceNumber", p),*/ Upcdt.Rows[p]["AlbumPrimaryArtist"])
                                 , new XElement("LabelName", Upcdt.Rows[p]["ReleaseLabelName"])
                                 , new XElement("Title", new XAttribute("TitleType", "FormalTitle"), new XElement("TitleText", Upcdt.Rows[p]["VideoTitleText"])
                                 //, new XElement("SubTitle", "Live")
                                 )
                                 //, new XElement("Title", new XAttribute("TitleType", "DisplayTitle"), new XElement("TitleText", Upcdt.Rows[p]["VideoTitleText"]))
                                 , getDisplayAttrvalue(Upcdt.Rows[p]["AlbumPrimaryArtist"].ToString(), "AlbumPrimaryArtist")
                                 , getDisplayAttrvalue(Upcdt.Rows[p]["FeaturedArtist"].ToString(), "FeaturedArtist")
                                 //, new XElement("DisplayArtist", new XAttribute("SequenceNumber", "1"), new XElement("PartyName", new XElement("FullName", Upcdt.Rows[p]["AlbumPrimaryArtist"]))                                
                                 //, new XElement("ArtistRole", "MainArtist"))
                                 //, new XElement("RelatedRelease", new XElement("ReleaseId", new XElement("ProprietaryId", new XAttribute("Namespace", "DPID:PADPIDA2013011103E"), "M" + (p + 1)))
                                 //, new XElement("ReferenceTitle", new XElement("TitleText", Upcdt.Rows[p]["VideoTitleText"]))
                                 //, new XElement("ReleaseRelationshipType", new XAttribute("Namespace", "DPID:PADPIDA2013011103E"), new XAttribute("UserDefinedValue", "RelatedMovie"), "UserDefined")

                                 , new XElement("ParentalWarningType", "NotExplicit"));
                                XElement ResourceGroupParent = new XElement("ResourceGroup");
                                XElement ResourceSubParent = new XElement("ResourceGroup");
                                //XElement ResourceGroupChild = new XElement("SequenceNumber", "1");
                                XElement TitleElement = new XElement("Title", new XAttribute("TitleType", "GroupingTitle"), new XElement("TitleText", "Component 1"));
                                XElement SequenceNumber = new XElement("SequenceNumber", "1");
                                ResourceGroupParent.Add(ResourceSubParent);
                                ResourceSubParent.Add(TitleElement);
                                ResourceSubParent.Add(SequenceNumber);
                                // ResourceSubParent.Add(ResourceGroupChild);
                                int ImageSeqCount = 0;
                                for (int o = 1; o <= TrackCount; o++)
                                {
                                    new XElement("SequenceNumber", "1");
                                    XElement ResourceGroup = new XElement("ResourceGroupContentItem", new XElement("SequenceNumber", "1"), new XElement("ResourceType", "Video")
                                    , new XElement("ReleaseResourceReference", new XAttribute("ReleaseResourceType", "PrimaryResource"), "A" + o)
                                    , new XElement("LinkedReleaseResourceReference", new XAttribute("LinkDescription", "VideoScreenCapture"), "A" + (o + 1))
                                    );
                                    ResourceSubParent.Add(ResourceGroup);
                                    ImageSeqCount = o;
                                }
                                ImageSeqCount = ImageSeqCount + 1;
                                //XElement ResourceGroup1 = new XElement("ResourceGroupContentItem", new XElement("SequenceNumber", ImageSeqCount), new XElement("ResourceType", "Image")
                                //   , new XElement("ReleaseResourceReference", new XAttribute("ReleaseResourceType", "SecondaryResource"), "A" + ImageSeqCount));
                                new XElement("SequenceNumber", "1");
                                //XElement ResourceGroup1 = new XElement("ResourceGroupContentItem"/*, new XElement("SequenceNumber", "1")*/, new XElement("ResourceType", "Image")
                                //, new XElement("ReleaseResourceReference", new XAttribute("ReleaseResourceType", "SecondaryResource"), "A" + ImageSeqCount));
                                // ResourceGroupParent.Add(ResourceGroup1);
                                ReleaseDetailsByTerritory.Add(ResourceGroupParent);
                                // 01112019 commented ReleaseDetailsByTerritory.Add(ResourceGroupParent);
                                XElement Genre = new XElement("Genre", new XElement("GenreText", Upcdt.Rows[p]["GenreText"]), new XElement("SubGenre", Upcdt.Rows[p]["SubGenre"]));
                                XElement OriginalReleaseDate = new XElement("ReleaseDate", Convert.ToDateTime(Upcdt.Rows[p]["OriginalReleaseDate"]).ToString("yyyy-MM-dd"));
                                //XElement Duration = new XElement("Duration", timestamp(Span1.ToString()));

                                XElement PLine = new XElement("PLine", new XElement("Year", Upcdt.Rows[p]["pYear"])
                                     , new XElement("PLineText", Upcdt.Rows[p]["PLineText"]));
                                XElement CLine = new XElement("CLine", new XElement("Year", Upcdt.Rows[p]["cYear"])
                                  , new XElement("CLineText", Upcdt.Rows[p]["CLineText"]));
                                XElement GlobalOriginalReleaseDate = new XElement("GlobalOriginalReleaseDate", Convert.ToDateTime(Upcdt.Rows[p]["ReleaseDate"]).ToString("yyyy-MM-dd"));


                                ReleaseDetailsByTerritory.Add(Genre);
                                ReleaseDetailsByTerritory.Add(OriginalReleaseDate);
                                ReleaseTag.Add(ReleaseDetailsByTerritory);
                                // ReleaseTag.Add(Duration);
                                ReleaseTag.Add(PLine);
                                ReleaseTag.Add(CLine);
                                ReleaseTag.Add(GlobalOriginalReleaseDate);
                                // ReleaseTag.Add(CLine);
                                ReleaseListTag.Add(ReleaseTag);

                            }

                            else if (p == TrackCount)
                            {
                                XElement Release = new XElement("Release", new XElement("ReleaseId", new XElement("ISRC", Upcdt.Rows[p - 1]["ISRC_video"])
                                //    , new XElement("CatalogNumber", new XAttribute("Namespace", "DPID:"), Upcdt.Rows[p - 1]["GGL_USR_ID"].ToString().Substring((Upcdt.Rows[p - 1]["GGL_USR_ID"].ToString().Length - 4), 4) + "_01" )
                                    )

                                      , new XElement("ReleaseReference", "R" + p)

                                          , new XElement("ReferenceTitle", new XElement("TitleText", Upcdt.Rows[p - 1]["TTitleText"])
                                          //, new XElement("SubTitle", "Live")
                                          )
                                          , new XElement("ReleaseResourceReferenceList"
                                          , new XElement("ReleaseResourceReference", new XAttribute("ReleaseResourceType", "PrimaryResource"), "A" + p)
                                          , new XElement("ReleaseResourceReference", new XAttribute("ReleaseResourceType", "SecondaryResource"), "A" + (p + 1))
                                          )
                                          , new XElement("ReleaseType", "VideoTrackRelease")
                                          , new XElement("ReleaseDetailsByTerritory", new XElement("TerritoryCode", Upcdt.Rows[p - 1]["TerritoryCode"])
                                          , new XElement("DisplayArtistName", Upcdt.Rows[p - 1]["DisplayArtist_FullName"])
                                          , new XElement("LabelName", Upcdt.Rows[p - 1]["LabelName"])
                                          , new XElement("Title", new XAttribute("TitleType", "FormalTitle"), new XElement("TitleText", Upcdt.Rows[p - 1]["TTitleText"])
                                          //, new XElement("SubTitle", "Live")
                                          )
                                         // , new XElement("Title", new XAttribute("TitleType", "DisplayTitle"), new XElement("TitleText", Upcdt.Rows[p - 1]["TTitleText"]))
                                                      , getDisplayAttrvalue(Upcdt.Rows[p - 1]["DisplayArtist_FullName"].ToString(), "DisplayArtist_FullName")
                                 , getDisplayAttrvalue(Upcdt.Rows[p - 1]["FeaturedArtist"].ToString(), "FeaturedArtist")
                                         //, new XElement("DisplayArtist", new XElement("PartyName", new XElement("FullName", Upcdt.Rows[p - 1]["DisplayArtist_FullName"]))
                                         //, new XElement("ArtistRole", "MainArtist"))
                                         , new XElement("RelatedRelease"
                                         , new XElement("ReleaseId"
                                         , new XElement("ISRC", "INS922100085")
                                         , new XElement("ICPN", new XAttribute("IsEan", "false"), "0194548008632"))
                                         , new XElement("ReferenceTitle", new XElement("TitleText", Upcdt.Rows[p - 1]["TTitleText"]))
                                          , new XElement("ReleaseRelationshipType", "IsEquivalentToAudio")
                                         )

                                         , new XElement("ParentalWarningType", "NotExplicit")
                                                     , new XElement("ResourceGroup", new XElement("ResourceGroup", new XElement("Title", new XAttribute("TitleType", "GroupingTitle"), new XElement("TitleText", "Component 1"))
                                                                                   //, new XElement("SequenceNumber", p)
                                                                                   //, getResourseList(TrackCount)
                                                                                   , new XElement("SequenceNumber", "1")
                                             , new XElement("ResourceGroupContentItem", new XElement("SequenceNumber", "1"), new XElement("ResourceType", "Video")
                                         , new XElement("ReleaseResourceReference", new XAttribute("ReleaseResourceType", "PrimaryResource"), "A" + p)
                                         , new XElement("LinkedReleaseResourceReference", new XAttribute("LinkDescription", "VideoScreenCapture"), "A" + (p + 1))
                                         )
                                                     ))

                                         , new XElement("Genre", new XElement("GenreText", Upcdt.Rows[p - 1]["GenreText"]), new XElement("SubGenre", Upcdt.Rows[p - 1]["SubGenre"]))
                                         , new XElement("ReleaseDate", Convert.ToDateTime(Upcdt.Rows[p - 1]["OriginalReleaseDate"]).ToString("yyyy-MM-dd"))
                                         , new XElement("Keywords", Upcdt.Rows[p - 1]["Keywords"])
                                         )
                                         //  , new XElement("Duration", "PT" + (DateTime.Parse(Upcdt.Rows[p - 1]["Duration"].ToString()).Hour).ToString() + "H" + (DateTime.Parse(Upcdt.Rows[p - 1]["Duration"].ToString()).ToString("mm")) + "M" + (DateTime.Parse(Upcdt.Rows[p - 1]["Duration"].ToString()).ToString("ss")) + "S")
                                         , new XElement("PLine", new XElement("Year", Upcdt.Rows[p - 1]["pYear"])
                                         , new XElement("PLineText", "(P)" + Upcdt.Rows[p - 1]["PLineText"]))
                                         , new XElement("CLine", new XElement("Year", Upcdt.Rows[p - 1]["pYear"])
                                         , new XElement("CLineText", "(P)" + Upcdt.Rows[p - 1]["PLineText"]))

                                         , new XElement("GlobalOriginalReleaseDate", Convert.ToDateTime(Upcdt.Rows[p - 1]["ReleaseDate"]).ToString("yyyy-MM-dd"))
                                         );
                                ReleaseListTag.Add(Release);
                            }

                            else
                            {
                                XElement Release = new XElement("Release", new XElement("ReleaseId", new XElement("ISRC", Upcdt.Rows[p - 1]["ISRC_video"])
                                                        , new XElement("CatalogNumber", new XAttribute("Namespace", "DPID"), Upcdt.Rows[p]["GGL_USR_ID"].ToString().Substring((Upcdt.Rows[p]["GGL_USR_ID"].ToString().Length - 4), 4) + "_0" + a)
                                    )
                                          , new XElement("ReleaseReference", "R" + p), new XElement("ReferenceTitle", new XElement("TitleText", Upcdt.Rows[p - 1]["TTitleText"]))
                                          , new XElement("ReleaseResourceReferenceList", new XElement("ReleaseResourceReference", new XAttribute("ReleaseResourceType", "PrimaryResource"), "A" + p))
                                          , new XElement("ReleaseType", "TrackRelease")
                                          , new XElement("ReleaseDetailsByTerritory", new XElement("TerritoryCode", Upcdt.Rows[p - 1]["TerritoryCode"])
                                          , new XElement("DisplayArtistName", Upcdt.Rows[p - 1]["DisplayArtist_FullName"])
                                          , new XElement("LabelName", Upcdt.Rows[p - 1]["LabelName"])
                                          , new XElement("Title", new XAttribute("TitleType", "FormalTitle"), new XElement("TitleText", Upcdt.Rows[p - 1]["TTitleText"]))
                                          , new XElement("Title", new XAttribute("TitleType", "DisplayTitle"), new XElement("TitleText", Upcdt.Rows[p - 1]["TTitleText"]))
                                         , new XElement("DisplayArtist", new XElement("PartyName", new XElement("FullName", Upcdt.Rows[p - 1]["DisplayArtist_FullName"]))
                                         , new XElement("ArtistRole", "MainArtist"))
                                         //, new XElement("DisplayArtist", new XElement("PartyName", new XElement("FullName", Upcdt.Rows[p - 1]["DisplayArtist_FullName"]))
                                         //, new XElement("ArtistRole", "MainArtist"))
                                         , new XElement("ParentalWarningType", "NotExplicit")
                                         //,new XElement("SequenceNumber","1")
                                         , new XElement("ResourceGroup", new XElement("ResourceGroupContentItem"/*, new XElement("SequenceNumber", 1)*/, new XElement("ResourceType", "SoundRecording"), new XElement("ReleaseResourceReference", new XAttribute("ReleaseResourceType", "PrimaryResource"), "A" + p)))
                                            , new XElement("Genre", new XElement("GenreText", Upcdt.Rows[p - 1]["GenreText"]), new XElement("SubGenre"))
                                          , new XElement("OriginalReleaseDate", Convert.ToDateTime(Upcdt.Rows[p]["OriginalReleaseDate"]).ToString("yyyy-MM-dd"))
                                         )
                                          , new XElement("Duration", "PT" + (DateTime.Parse(Upcdt.Rows[p - 1]["Duration"].ToString()).Hour).ToString() + "H" + (DateTime.Parse(Upcdt.Rows[p - 1]["Duration"].ToString()).ToString("mm")) + "M" + (DateTime.Parse(Upcdt.Rows[p - 1]["Duration"].ToString()).ToString("ss")) + "S")
                                         , new XElement("PLine", new XElement("Year", Upcdt.Rows[p - 1]["pYear"])
                                         , new XElement("PLineText", Upcdt.Rows[p - 1]["PLineText"]))
                                             , new XElement("CLine", new XElement("Year", Upcdt.Rows[p - 1]["cYear"])
                                         , new XElement("CLineText", Upcdt.Rows[p - 1]["CLineText"]))
                                         //, new XElement("GlobalOriginalReleaseDate", Convert.ToDateTime(Upcdt.Rows[p]["ReleaseDate"]).ToString("yyyy-MM-dd"))
                                         );
                                ReleaseListTag.Add(Release);
                            }


                        }
                        // End Of Release List========================================================
                        // Start of deal list===============================================================================================

                        XElement DealList = new XElement("DealList");
                        XElement ReleaseDealTag = null;
                        List<XElement> DealReleaseReference = new List<XElement>();
                        for (int q = 0; q <= TrackCount; q++)
                        {

                            if (q == 0)
                            {
                                ReleaseDealTag = new XElement("ReleaseDeal", new XElement("DealReleaseReference", "R" + (q))
                               , new XElement("Deal", new XElement("DealTerms", new XElement("CommercialModelType", "AdvertisementSupportedModel")
                               , new XElement("Usage"
                                , new XElement("UseType", "OnDemandStream")
                                , new XElement("UseType", "NonInteractiveStream")
                               //  , new XElement("UseType", (item == "Saavn" ? "NonInteractiveStream" : "Stream"))
                               )
                               , new XElement("TerritoryCode", Upcdt.Rows[q]["TerritoryCode"])
                               , new XElement("ValidityPeriod", new XElement("StartDate", Convert.ToDateTime(Upcdt.Rows[q]["DealStartDate"]).ToString("yyyy-MM-dd")))
                            //   , new XElement("RightsClaimPolicy", new XElement("RightsClaimPolicyType", "Monetize"))
                               ))
                                  , new XElement("Deal", new XElement("DealTerms", new XElement("CommercialModelType", "SubscriptionModel")
                               , new XElement("Usage"
                                , new XElement("UseType", "ConditionalDownload")
                                , new XElement("UseType", "OnDemandStream")
                                , new XElement("UseType", "NonInteractiveStream")
                               )
                               , new XElement("TerritoryCode", Upcdt.Rows[q]["TerritoryCode"])
                               , new XElement("ValidityPeriod", new XElement("StartDate", Convert.ToDateTime(Upcdt.Rows[q]["DealStartDate"]).ToString("yyyy-MM-dd")))
                               //, new XElement("RightsClaimPolicy", new XElement("RightsClaimPolicyType", "Monetize"))
                               )
                               )
                               );
                                DealList.Add(ReleaseDealTag);
                            }
                            else
                            {
                                //ReleaseDealTag = new XElement("ReleaseDeal", new XElement("DealReleaseReference", "R" + (q))
                                //, new XElement("Deal", new XElement("DealTerms", new XElement("CommercialModelType", "AdvertisementSupportedModel")
                                //, new XElement("Usage"
                                // , new XElement("UseType", "OnDemandStream")
                                // , new XElement("UseType", "NonInteractiveStream")
                                ////  , new XElement("UseType", (item == "Saavn" ? "NonInteractiveStream" : "Stream"))
                                //)
                                //, new XElement("TerritoryCode", Upcdt.Rows[q]["TerritoryCode"])
                                //, new XElement("ValidityPeriod", new XElement("StartDate", Convert.ToDateTime(Upcdt.Rows[q]["DealStartDate"]).ToString("yyyy-MM-dd")))
                                ////   , new XElement("RightsClaimPolicy", new XElement("RightsClaimPolicyType", "Monetize"))
                                //))
                                //   , new XElement("Deal", new XElement("DealTerms", new XElement("CommercialModelType", "SubscriptionModel")
                                //, new XElement("Usage"
                                // , new XElement("UseType", "ConditionalDownload")
                                // , new XElement("UseType", "OnDemandStream")
                                // , new XElement("UseType", "NonInteractiveStream")
                                //)
                                //, new XElement("TerritoryCode", Upcdt.Rows[q]["TerritoryCode"])
                                //, new XElement("ValidityPeriod", new XElement("StartDate", Convert.ToDateTime(Upcdt.Rows[q]["DealStartDate"]).ToString("yyyy-MM-dd")))
                                ////, new XElement("RightsClaimPolicy", new XElement("RightsClaimPolicyType", "Monetize"))
                                //)
                                //)
                                //);

                                //DealList.Add(ReleaseDealTag);

                            }


                        }

                        //XElement Deal = new XElement("Deal", new XElement("DealTerms", new XElement("CommercialModelType", "AdvertisementSupportedModel")
                        // , new XElement("CommercialModelType", "SubscriptionModel")
                        //     , new XElement("Usage"
                        //     , new XElement("UseType", "OnDemandStream"))
                        //     , new XElement("TerritoryCode", "Worldwide")
                        //     , new XElement("ValidityPeriod", new XElement("StartDate", DateTime.Now.ToString("yyyy-MM-dd")))));
                        //XElement Deallast = new XElement("Deal", new XElement("DealReference", "YT_MATCH_POLICY:Monetize in all countries"));
                        // ReleaseDealTag.Add(DealReleaseReference);
                        //ReleaseDealTag.Add(Deal);
                        // ReleaseDealTag.Add(Deallast);
                        //  DealList.Add(ReleaseDealTag);
                        // End Of Deal List=================================================================================================
                        //Add All nodes


                        XNamespace ernm = "http://ddex.net/xml/ern/382";
                        XNamespace second = "http://www.w3.org/2001/XMLSchema-instance";
                        XNamespace schemaLocation = XNamespace.Get("http://ddex.net/xml/ern/382 http://ddex.net/xml/ern/382/release-notification.xsd");
                        XNamespace xsiNs = "http://www.w3.org/2001/XMLSchema-instance";

                        XElement root = new XElement(ernm + "NewReleaseMessage", new XAttribute(XNamespace.Xmlns + "ernm", ernm), new XAttribute(XNamespace.Xmlns + "xs", second)
                        //, new XAttribute("LanguageAndScriptCode", "en")
                        , new XAttribute(xsiNs + "schemaLocation", schemaLocation)
                        , new XAttribute("MessageSchemaVersionId", "ern/382")
                        // , new XAttribute("ReleaseProfileVersionId", "CommonReleaseTypesTypes/14/AudioAlbumMusicOnly")
                        , new XAttribute("LanguageAndScriptCode", "en")
                        );
                        root.Add(guvera);
                        // XElement root = new XElement("root", "root");
                        //root.Add(UpdateNode);
                        Resource.Add(ImageTag);
                        root.Add(Resource);
                        root.Add(ReleaseListTag);
                        root.Add(DealList);
                        //string FileName = (DateTime.Now).ToString("yyyyMMddhhmmss");
                        XDocument doc = new XDocument(new XDeclaration("1.0", "utf8", "no"), root);



                        //var files = from file in Directory.EnumerateFiles(Upcdt.Rows[0]["TrackPath"].ToString()) select file;
                        //foreach (var file in files)
                        //{

                        //}

                        doc.Save(@"E:\JioSavaanVideo\" + item + "\\" + foldername + "\\" + FileName + "\\" + FileName + ".xml");
                        //  Console.ReadLine();

                        ///------------------------------------creating batch file-------------------------------
                        ///
                        XNamespace ernmbatch = "http://ddex.net/xml/ern/341";
                        XNamespace secondbatch = "http://ddex.net/xml/2011/echo/12";
                        XNamespace schemaLocationbatch = XNamespace.Get("http://ddex.net/xml/ern/382 http://ddex.net/xml/ern/382/release-notification.xsd");
                        XNamespace xsiNsbatch = "http://www.w3.org/2001/XMLSchema-instance";



                        if (item == "Spotify")
                        {
                            XElement rootbatch = new XElement(secondbatch + "ManifestMessage"
                                , new XAttribute(XNamespace.Xmlns + "ernm", ernmbatch)
                                , new XAttribute(XNamespace.Xmlns + "echo", secondbatch)
                                , new XAttribute(XNamespace.Xmlns + "amep", "http://ddex.net/xml/2011/amep/12")
                            , new XAttribute(XNamespace.Xmlns + "xs", second)
                            , new XAttribute(XNamespace.Xmlns + "ds", "http://www.w3.org/2000/09/xmldsig#")
                            , new XAttribute("MessageVersionId", "1.2")
                             , new XAttribute(xsiNs + "schemaLocation", schemaLocation)
                            );
                            XElement MessageHeader = new XElement("MessageHeader", new XElement("MessageSender", new XElement("PartyId", "PADPIDA2013011103E")
                            , new XElement("PartyName", new XElement("FullName", "Seven Colors")))
                                , new XElement("MessageRecipient", new XElement("PartyId", "PADPIDA2011072101T")
                                , new XElement("PartyName", new XElement("FullName", "Spotify"))
                                )
                                , new XElement("MessageCreatedDateTime", DateTime.UtcNow.ToString("s") + "+05:30")
                                );
                            XElement IsTestFlag = new XElement("IsTestFlag", "false");
                            XElement RootDirectory = new XElement("RootDirectory", "/" + foldername);
                            XElement NumberOfMessages = new XElement("NumberOfMessages", "1");
                            XElement MessageInBatch = new XElement("MessageInBatch", new XElement("MessageType", "NewReleaseMessage")
                                , new XElement("MessageId", "4bd15d93feb14e0293143d6cccbf5765")
                                , new XElement("URL", "/" + foldername + "\\" + FileName + "\\" + FileName + ".xml")
                                , new XElement("IncludedReleaseId", new XElement("ICPN", new XAttribute("IsEan", "false"), FileName))
                                , new XElement("DeliveryType", "NewReleaseDelivery")
                                , new XElement("ProductType", "AudioProduct")
                                , new XElement("HashSum", new XElement("HashSum", GetHashSum(@"E:\JioSavaanVideo\" + item + "\\" + foldername + "\\" + FileName + "\\" + FileName + ".xml")), new XElement("HashSumAlgorithmType", "MD5"))
                                );

                            rootbatch.Add(MessageHeader);
                            rootbatch.Add(IsTestFlag);
                            rootbatch.Add(RootDirectory);
                            rootbatch.Add(NumberOfMessages);
                            rootbatch.Add(MessageInBatch);
                            XDocument docbatch = new XDocument(new XDeclaration("1.0", "UTF8", "yes"), rootbatch);
                            docbatch.Save(@"E:\JioSavaanVideo\" + item + "\\" + foldername + "\\BatchComplete_" + foldername + ".xml");
                        }
                        else
                        {
                            doc.Save(@"E:\JioSavaanVideo\" + item + "\\" + foldername + "\\BatchComplete_" + foldername + ".xml");
                        }
                        System.Threading.Thread.Sleep(1000);
                    }
                    Console.WriteLine("JioSavaanVideo AND BATCH FILE --" + item + "-- CREATED SUCCESFULLY");

                    // Console.ReadLine();
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(ex, true);
                System.Diagnostics.StackFrame sf = st.GetFrame(st.FrameCount - 1);
                int irow = sf.GetFileLineNumber();
                int icol = sf.GetFileColumnNumber();
                Console.WriteLine(ex.Message + "\r\n" + "Error at line No.:-" + irow.ToString());
                Console.Read();
            }
        }

        public static string GetHashSum(string path)
        {
            string str = "";
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(path))
                {
                    var data = md5.ComputeHash(stream);
                    str = BitConverter.ToString(data).Replace("-", "").ToLowerInvariant();

                }
            }
            return str;
        }
        public static string timestamp(string id)
        {
            string[] str = id.Split(':');
            string value = "PT" + str[0] + "H";
            value += str[1] + "M";
            value += str[2] + "S";
            return value;
        }
        public static XElement[] getDisplayAttrvalue(string value, string elementType)
        {
            var data = value.Split(',');
            int seq = 1;

            XElement[] xElement = new XElement[data.Length];
            if (data.Length > 0)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] != "")
                    {
                        if (elementType == "FeaturedArtist" && !string.IsNullOrEmpty(value))
                        {
                            xElement[i] = new XElement("DisplayArtist", new XAttribute("SequenceNumber", seq)
                     , new XElement("PartyName", new XElement("FullName", value))
                     , new XElement("ArtistRole", "FeaturedArtist"));
                        }

                        else
                        {
                            xElement[i] = new XElement("DisplayArtist", new XAttribute("SequenceNumber", seq)
                     , new XElement("PartyName", new XElement("FullName", data[i]))
                     , new XElement("ArtistRole", "MainArtist"));
                        }



                    }
                    else if (elementType == "FeaturedArtist" && string.IsNullOrEmpty(value))
                    {
                        //   xElement[i] = new XElement("DisplayArtist"
                        //, new XElement("PartyName", new XElement("FullName", value))
                        //, new XElement("ArtistRole", "FeaturedArtist"));
                    }
                    else
                    {
                        xElement[i] = new XElement("DisplayArtist", new XAttribute("SequenceNumber", seq)
                     , new XElement("PartyName", new XElement("FullName", data[i]))
                     , new XElement("ArtistRole", "MainArtist"));
                    }
                    seq++;
                }
            }
            else
            {
                xElement = null;
            }
            return xElement;

        }
        public static XElement[] getAttrvalue(string value, string elementType, string parentNodeName, string childNodeName)
        {
            var data = value.Split(',');
            int seq = 1;

            XElement[] xElement = new XElement[data.Length];
            if (data.Length > 0)
            {
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] != "")
                    {
                        if (elementType == "Lyricist")
                        {
                            xElement[i] = new XElement(parentNodeName, new XAttribute("SequenceNumber", "2"), new XElement("PartyName", new XElement("FullName", value))
                               , new XElement(childNodeName, elementType));
                        }//Composer
                        else if (elementType == "Producer")
                        {
                            xElement[i] = new XElement(parentNodeName/*, new XAttribute("SequenceNumber", "1")*/, new XElement("PartyName", new XElement("FullName", data[i]))
                               , new XElement(childNodeName, elementType));
                        }
                        else if (elementType == "Composer")
                        {
                            xElement[i] = new XElement(parentNodeName, new XAttribute("SequenceNumber", "1"), new XElement("PartyName", new XElement("FullName", data[i]))
                               , new XElement(childNodeName, elementType));
                        }
                        else
                        {
                            xElement[i] = new XElement(parentNodeName,/* new XAttribute("SequenceNumber", seq),*/ new XElement("PartyName", new XElement("FullName", data[i]))
                               , new XElement(childNodeName, elementType));
                        }


                    }
                    else
                    {
                        xElement = null;
                    }
                    seq++;
                }
            }
            else
            {
                xElement = null;
            }
            return xElement;

        }

        public static XElement[] getResourseList(int count)
        {
            XElement[] xElement = new XElement[count];
            try
            {

                int value = 1;
                for (int o = 0; o < count; o++)
                {
                    //new XElement("SequenceNumber", value);
                    xElement[o] = new XElement("ResourceGroupContentItem", /*new XElement("SequenceNumber", value),*/ new XElement("ResourceType", "SoundRecording")
                     , new XElement("ReleaseResourceReference", new XAttribute("ReleaseResourceType", "PrimaryResource"), "A" + value));
                    value++;
                }

            }
            catch (Exception ex)
            {

            }
            return xElement;

        }
    }
}
