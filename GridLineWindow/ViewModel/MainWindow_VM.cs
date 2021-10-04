using MVVM.Tools;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using GridLineWindow.Model;
using System.Windows.Controls.Primitives;
using GridLineWindow.Common.Helpers;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Xml;

namespace GridLineWindow.ViewModel
{
    class MainWindow_VM : ReactiveObject
    {
        public MainWindow_VM()
        {
            NewBtnCommand = new RelayCommand(o => NewBtnClick(o));
            NewGroupCommand = new RelayCommand(o => NewGroupClick(o));
            EditBtnCommand = new RelayCommand(o => EditBtnClick(o));
            DeleteBtnCommand = new RelayCommand(o => DeleteBtnClick(o));
            SaveBtnCommand = new RelayCommand(o => SaveBtnClick(o));
            SaveProjectCommand = new RelayCommand(o => SaveProjectClick(o));
            OpenProjectCommand = new RelayCommand(o => OpenProjectClick(o));
            ToggleSidebarCommand = new RelayCommand(o => ToggleSidebarClick(o));
            SelectionCommand = new RelayCommand(o => ItemClick(o));

        }
        public int Left
        {
            get => (int)((float)System.Windows.SystemParameters.PrimaryScreenWidth - WindowWidth) / 2;
        }
        public int Top
        {
            get => (int)((float)System.Windows.SystemParameters.PrimaryScreenHeight - WindowHeight) / 2;
        }

        private ObservableCollection<GroupItem> unitItems = new ObservableCollection<GroupItem>();
        public ObservableCollection<GroupItem> UnitItems
        {
            get => unitItems;
            set => this.RaiseAndSetIfChanged(ref unitItems, value);
        }

        private float windowWidth = 1800;
        public float WindowWidth
        {
            get => windowWidth;
        }

        private float windowHeight = 900;
        public float WindowHeight
        {
            get => windowHeight;
        }

        private string[] fileMenuItems = { "Open", "Open Recent", "Save", "Save as", "Depulicate" };
        public string[] FileMenuItems
        {
            get => fileMenuItems;
        }

        private ObservableCollection<Category> categories = new ObservableCollection<Category>();
        public ObservableCollection<Category> Categories
        {
            get
            {
                if(categories.Count == 0)
                {
                    categories.Add(new Category
                    {
                        ID = 1,
                        Name = "Favorite",
                        Url = "/Resources/background.jpg"
                    });
                    categories.Add(new Category
                    {
                        ID = 1,
                        Name = "Chart",
                        Url = "/Resources/background.jpg"
                    });
                    categories.Add(new Category
                    {
                        ID = 1,
                        Name = "Folder",
                        Url = "/Resources/background.jpg"
                    });
                    categories.Add(new Category
                    {
                        ID = 1,
                        Name = "Setting",
                        Url = "/Resources/background.jpg"
                    });

                }
                return categories;
            }
        }

        private ObservableCollection<GroupItem> groups = new ObservableCollection<GroupItem>();
        public ObservableCollection<GroupItem> Groups
        {
            get
            {
                if (groups.Count > 0) return groups;
                 return groups;
            }
            set => this.RaiseAndSetIfChanged(ref groups, value);
        }

        private void ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            GroupItem selection = this.UnitItems.Where(u => u.ID == ((GroupItem)sender).ID).First();
            selection.IsActive = true;
        }

        private bool isChecked = true;
        public bool IsChecked
        {
            get => isChecked;
            set
            {
                this.RaiseAndSetIfChanged(ref isChecked, value);
                if (isChecked == false) StartPoint = EndPoint = new Point(0, 0);
                else
                {
                    StartPoint = new Point(0, cellWidth);
                    EndPoint = new Point(cellWidth, 0);
                }
                
            }
        }
        private int gridCount = 3;
        public int GridCount
        {
            get => gridCount;
            set => this.RaiseAndSetIfChanged(ref gridCount, value);
        }
        private int groupImgWidth = 200;
        public int GroupImgWidth
        {
            get => groupImgWidth;
            set => this.RaiseAndSetIfChanged(ref groupImgWidth, value);
        }
        private float sliderValue = 25f;
        public float SliderValue
        {
            get => sliderValue;
            set
            {
                this.RaiseAndSetIfChanged(ref sliderValue, value);
                if (!IsChecked) return;
                this.RaiseAndSetIfChanged(ref cellWidth, (int)value / 2);
                if(SliderValue < 20)
                {
                    GridCount = 5;
                }else if(SliderValue < 30)
                {
                    GridCount = 4;
                }
                else if (SliderValue < 40)
                {
                    GridCount = 3;
                }
                else if (SliderValue < 50)
                {
                    GridCount = 2;

                }
                StartPoint = new Point(0, cellWidth);
                EndPoint = new Point(cellWidth, 0);
                GridRect = new Rect(0, 0, cellWidth, cellWidth);
                this.RaiseAndSetIfChanged(ref startPoint, new Point(0, cellWidth));
                this.RaiseAndSetIfChanged(ref endPoint, new Point(cellWidth, 0));
            }
        }

        private int cellWidth = 20;
        public int CellWidth
        {
            get => cellWidth;
            set => this.RaiseAndSetIfChanged(ref cellWidth, value);
        }

        private int[] viewPort = { 0, 0, 20, 20 };
        public int[] ViewPort
        {
            get => viewPort;
            set => this.RaiseAndSetIfChanged(ref viewPort, value);
        }

        private Rect gridRect = new Rect(0, 0, 20, 20);
        public Rect GridRect
        {
            get => gridRect;
            set => this.RaiseAndSetIfChanged(ref gridRect, value);
        }

        private Rect backgroundRect = new Rect(0, 0, 1100, 1000);
        public Rect BackgroundRect
        {
            get => backgroundRect;
            set => this.RaiseAndSetIfChanged(ref backgroundRect, value);
        }

        private Point startPoint = new Point(0, 20);
        public Point StartPoint
        {
            get => startPoint;
            set => this.RaiseAndSetIfChanged(ref startPoint, value);
        }
        private Point endPoint = new Point(20, 0);
        public Point EndPoint
        {
            get => endPoint;
            set => this.RaiseAndSetIfChanged(ref endPoint, value);
        }
        public ICommand NewBtnCommand { get; set; }
        private void NewBtnClick(object sender)
        {
            this.Groups.Add(new GroupItem { 
                            Name = "", 
                            Url = "/Resources/avatar4.png", 
                            ID = Groups.Count,
                            Action = SelectionCommand,
                Margin = new Thickness(10 * Groups.Count, 10 * Groups.Count, 0, 0),
                TimeSpan = DateTime.Now });

            if(Groups.Count > 9 && Groups.Count % 3 == 1)
            {
                BackgroundRect = new Rect(0, 0, 1100, 300*(Groups.Count/3 + 1) + 50);
            }
        }
        public ICommand NewGroupCommand { get; set; }
        private void NewGroupClick(object sender)
        {
            var messageBoxResult = System.Windows.MessageBox.Show("Would you like to save your project?", "confirm", MessageBoxButton.YesNo);
            if(messageBoxResult == MessageBoxResult.Yes)
            {
                SaveProjectClick("");
                return;
            }
            Groups = new ObservableCollection<GroupItem>();
        }
        public ICommand SelectionCommand { get; set; }
        private void ItemClick(object sender)
        {
            SelectedGroupItem = (GroupItem)sender;
            MarginLeft = (int)SelectedGroupItem.Margin.Left;
            MarginTop = (int)SelectedGroupItem.Margin.Top;
        }

        public ICommand EditBtnCommand { get; set; }
        private void EditBtnClick(object sender)
        {
            IsEdit = Visibility.Visible;
            IsOnlyShow = isEdit == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private int marginLeft = 0;
        public int MarginLeft
        {
            get => marginLeft;
            set => this.RaiseAndSetIfChanged(ref marginLeft, value);
        }
        public int marginTop = 0;
        public int MarginTop
        {
            get => marginTop;
            set => this.RaiseAndSetIfChanged(ref marginTop, value);
        }
        public string backgroundColor = "";
        public string BackgroundColor
        {
            get => backgroundColor;
            set => this.RaiseAndSetIfChanged(ref backgroundColor, value);
        }
        public ICommand SaveBtnCommand { get; set; }
        private void SaveBtnClick(object sender)
        {
            selectedGroupItem.Margin = new Thickness(MarginLeft, MarginTop, 0, 0);
            this.RaiseAndSetIfChanged(ref groups, Groups);
            IsEdit = Visibility.Hidden;
            IsOnlyShow = Visibility.Visible;
        }
        public ICommand ToggleSidebarCommand { get; set; }
        private void ToggleSidebarClick(object sender)
        {
            ShowSidebar = ShowSidebar == Visibility.Visible ? Visibility.Hidden : Visibility.Visible ;
        }
        public List<GroupItem> itms = new List<GroupItem>();
        public ICommand SaveProjectCommand { get; set; }
        private void SaveProjectClick(object sender)
        {
            //using (var sw = new StreamWriter("D:\\sample.xml"))
            //{
            //    var serializer = new XmlSerializer(typeof(XmlGroupTemplate));
            //    serializer.Serialize(sw, o: new XmlGroupTemplate { Groups = "sdfa" }); ;
            //}
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "project_1.xml";
            saveFileDialog.ShowDialog();
            
            if (saveFileDialog.FileName != "")
            {
                List<GroupItem> source = new List<GroupItem>(Groups);

                XmlSerializer serialiser = new XmlSerializer(typeof(List<GroupItem>));

                // Create the TextWriter for the serialiser to use
                TextWriter filestream = new StreamWriter(saveFileDialog.FileName);

                //write to the file
                serialiser.Serialize(filestream, source);

                // Close the file
                filestream.Close();
            }

            
        }

        public ICommand OpenProjectCommand { get; set; }
        private void OpenProjectClick(object sender)
        {
            OpenFileDialog openfileDialog = new OpenFileDialog();
            if(openfileDialog.ShowDialog() != DialogResult.Cancel)
            {
                string sourceFolderPath = openfileDialog.FileName;


                if (File.Exists(openfileDialog.FileName))
                {
                    List<GroupItem> loadGroups = new List<GroupItem>();

                    XmlTextReader reader = new XmlTextReader(openfileDialog.FileName);

                    object myXmlClass = (object)(new XmlSerializer(typeof(List<GroupItem>))).Deserialize(reader);

                    loadGroups = (List<GroupItem>)myXmlClass;

                    Groups = new ObservableCollection<GroupItem>(loadGroups);
                }
            }
        }
        public ICommand DeleteBtnCommand { get; set; }


        private void DeleteBtnClick(object selection)
        {
            Groups.Remove(SelectedGroupItem);
            this.RaiseAndSetIfChanged(ref groups, groups);
        }

        private GroupItem selectedGroupItem = null;
        public GroupItem SelectedGroupItem
        {
            get => selectedGroupItem;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedGroupItem, value);
                IsEdit = Visibility.Hidden;
                IsOnlyShow = Visibility.Visible;
                GroupActionState = value == null ? Visibility.Hidden : Visibility.Visible;
            }
        }

        public Visibility groupActionState = Visibility.Hidden;
        public Visibility GroupActionState
        {
            get
            {
                if (SelectedGroupItem == null) groupActionState = Visibility.Hidden;
                else groupActionState = Visibility.Visible;
                return groupActionState;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref groupActionState, value);
            }
        }

        private Visibility isEdit = Visibility.Hidden;
        public Visibility IsEdit
        {
            get
            {
                if (selectedGroupItem == null) isEdit = Visibility.Hidden;
                return isEdit;
            }
            set => this.RaiseAndSetIfChanged(ref isEdit, value);
        }
        private Visibility showSidebar = Visibility.Hidden;
        public Visibility ShowSidebar
        {
            get => showSidebar;
            set => this.RaiseAndSetIfChanged(ref showSidebar, value);
        }

        private Visibility isOnlyShow = Visibility.Hidden;
        public Visibility IsOnlyShow
        {
            get
            {
                if (selectedGroupItem == null) isOnlyShow = Visibility.Hidden;
                return isOnlyShow;
            }
            set => this.RaiseAndSetIfChanged(ref isOnlyShow, value);
        }
    }
}
