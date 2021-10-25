using System;
using Gtk;
using WebKit2;
using System.Collections.Generic;
using System.IO;
using Gdk;

public partial class MainWindow : Gtk.Window
{ //Warning: trying to load this file with ASCII will explode your PC
  //You son of a biŧ¢ħ, you load mango (https://github.com/hkalexling/mango) without wifi.
    bool incognito = false;
    List<WebView> webView = new List<WebView>();
    //Download download;
    bool loadpages = false;
    string working_dir = Directory.GetCurrentDirectory() + "/";
    string searchEngine;
    string initial_url = "https://duckduckgo.com";
    string[] args;
    //is a global variable bcs it doesn't want to work correctly
    Entry entry = new Entry();
    Gtk.Window dialog_change = new Gtk.Window("Select search engine");
    WebKit2.Settings settings = new WebKit2.Settings();
    Label labelTitle = new Label("Tab 1");

    //We begin the Main Window (Now with 30% more stupid comments)

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        dialog_change.Hide();
        //Now those ugly args are passed from one place to another
        //DIS IS BLACC MAGICC!
        args = Greending.MainClass.arguments;
        //Ah yes, it brocce
        //Ah yes, let's add some japanese here, some german there, some random characters...
        //Let's crash all the websites!
        settings.UserAgent = "€ggsßox/-2.0 (わOS; わøw€rわ© ©iŧröniuß 3.8; rv:24.0) W€ßKiŧGŧĸ/20160405 Gr€€nðıng/1.0";
        //Without UTF-8, the accents and other characters look like ßħiŧ
        //So Spanish/French/Japanese/German/etc... webpages load like they just ßħæŧ themselves
        //Fucc u ASCII
        settings.DefaultCharset = "UTF-8"; //tank for exist
        //Let it autoload images
        //Ah yes, 変態 for everyone
        settings.AutoLoadImages = true;
        //Let it run JavaScript
        //But it should not fucc itself,  liek it does
        settings.EnableJavascript = true;
        //Let us make the user experience smoother
        //Haha yes pun
        settings.EnableSmoothScrolling = true;

        //入 webView
         
        webView.Add(new WebView());
        //download.Destination = working_dir + "downloads";
        if (!loadpages)
        {
            //download.Failed += this.FailedDownload();
            webView[0].Settings = settings;
            //waskfjañfḱlñkjñkljíojklniomñonónoad the page
            if (args.Length != 0)
            {
                webView[0].LoadUri(args[0]);
            }
            else
            {
                webView[0].LoadUri(initial_url);
            }
            //Put the WebView into the VBox1
            notebook1.InsertPage(webView[0], labelTitle, 0);
            
            label1.Text = "Tab 1";
            webView[0].Show();
            notebook1.RemovePage(1);

        }
        else
        {
            string[] urls = File.ReadAllLines(working_dir + "conf/pages");
            webView[0].LoadUri(urls[0]);
            for (int i = 1; i < urls.Length; i++)
            {
                GenerateTab(urls[i]);
            }
            //notebook1.RemovePage(0);
            RefreshTabNames();
        }
        this.Resize(Convert.ToInt16(File.ReadAllText(working_dir + "conf/width.conf")), Convert.ToInt16(File.ReadAllText(working_dir + "conf/height.conf")));
        LoadConf();
        SetIconFromFile(working_dir + "icons/icon.png");

    }

    public void LoadConf()
    {   //Ah yes, "debug" (The fine art of Console.WriteLine)
        //Console.WriteLine(working_dir + "conf/menubar.conf");
        if (File.ReadAllText(working_dir + "conf/menubar.conf") == "true")
        {
            menubar1.Hide();
        }
        else
        {
            menubar1.Show();
        }
        searchEngine = File.ReadAllText(working_dir + "conf/searchengine.conf");
        if (args.Length == 0)
        {
            initial_url = File.ReadAllText(working_dir + "conf/home.conf");
        }
        else
        {
            initial_url = args[0];
        }

    }
    public string[] removeHashlines(string[] fileLines)
    {
        string[] result = fileLines;
        string tmp = "";
        for(int i = 0; i < result.Length; i++)
        {
            tmp = "";
            for(int j = 0; j < result[i].Length; j++)
            {
                if(result[i][j] == '#')
                {
                    break;
                }
                tmp += result[i][j];
            }
            result[i] = tmp;
        }
        return result;
    }

    public void SaveConf(string file, string value)
    { //Dis will not worcc, but i don't kër
        //Console.WriteLine(working_dir + "conf/" + file + ".conf");
        File.Delete(working_dir + "conf/" + file + ".conf");
        File.WriteAllText(working_dir + "conf/" + file + ".conf", value);
    }


    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        if (loadpages)
        {
            SavePages();
        }
        Application.Quit();
        a.RetVal = true;
    }

    protected void OpenURL(object sender, EventArgs e)
    {   //Dis code works for 99% of cases. For di otha ones, fucc off
        if (entry1.Text.Contains(".") && (entry1.Text.Contains("http://") || entry1.Text.Contains("https://")))
        {
            webView[notebook1.Page].LoadUri(entry1.Text); 
        }
        else if (entry1.Text.Contains("."))
        {
            string url = "http://" + entry1.Text;
            webView[notebook1.Page].LoadUri(url);
        }
        else if (entry1.Text.Contains("localhost"))
        {
            string url = "http://" + entry1.Text.Replace("localhost", "127.0.0.1");
            webView[notebook1.Page].LoadUri(url);
        }
        else
        {
            string entrytext = entry1.Text.Replace(" ", "+");
            string url = searchEngine + entrytext;
            webView[notebook1.Page].LoadUri(url);
        }
        webView[notebook1.Page].GrabFocus(); //So you don't have to waste your time clicking in other places
                                             //See how good I am?
        entry1.Text = webView[notebook1.Page].Uri;

    }

    protected void GoBack(object sender, EventArgs e)
    { //Go bacc
        webView[notebook1.Page].GoBack();
        webView[notebook1.Page].GrabFocus();
        entry1.Text = webView[notebook1.Page].Uri;

    }

    protected void GoForward(object sender, EventArgs e)
    { //Go forwar'
        webView[notebook1.Page].GoForward();
        webView[notebook1.Page].GrabFocus();
        entry1.Text = webView[notebook1.Page].Uri;

    }

    protected void Reload(object sender, EventArgs e)
    { //Rilod
        webView[notebook1.Page].Reload();
        webView[notebook1.Page].GrabFocus();
        entry1.Text = webView[notebook1.Page].Uri;

    }
    protected void Quit(object sender, EventArgs e)
    { //quitt
        if (loadpages)
        {
            SavePages();
        }
        Application.Quit();
        
    }
    
    protected void Open(object sender, EventArgs e)
    { //Opel the fayel (No offense Opel buyers)
        string url;
        FileChooserDialog chooser = new FileChooserDialog(
        "Please select a HTML file...",  //Ah yes, 100% foolproof code
        this, //Wat's dis
        FileChooserAction.Open, //Open my foken 変態's pliz
        "Cancel", ResponseType.Cancel, //NOOOOOOOOOOOOO, my 変態's!!!
        "Open", ResponseType.Accept); //Noice
        FileFilter filter = new FileFilter(); //NO MONODEVELOP, THE INITIALIZATION CAN'T BE SIMPLIFIED
        filter.Name = "HTML files";
        filter.AddPattern("*.html");
        chooser.AddFilter(filter);
        filter.Name = "All files";
        filter.AddPattern("*.*");
        chooser.AddFilter(filter);
        if (chooser.Run() == (int)ResponseType.Accept) //Plz ye
        {
            //Open the file for webpagin'
            //CAN'T FOKEN READ THIS CLUSTERFUCK
            //Shut it, the first line of this multi-comment
            //IT's bROĸeN
            System.IO.StreamReader file =
            System.IO.File.OpenText(chooser.Filename);
            url = chooser.Filename;
            webView[notebook1.Page].LoadUri("file://" + chooser.Filename);
            file.Close();
        } //End Plz ye
        chooser.Destroy();
        entry1.Text = webView[notebook1.Page].Uri;

    }

    protected void HideMenuBar(object sender, EventArgs e)
    {//Ah yes, enslaved morzilla foirefoccs
        menubar1.Hide();
        SaveConf("menubar", "true");

    }

    protected void OpenMenu(object sender, EventArgs e)
    {   //This shit's VERY BUGGY
        Dialog dialog = new Dialog();
        dialog.AddButton("Show menu bar", 0);
        dialog.AddButton("Hide menu bar", 1);
        dialog.AddButton("Change search engine", 2);
        webView[notebook1.Page].Settings = settings;
        dialog.Title = "Greending Settings";
        Pixbuf icon = new Pixbuf(working_dir + "icons/icon.png");
        dialog.Icon = icon;
        dialog.Show();
        //f(x) do u no worcc adekuately?
        //Machin nmo work?
        switch (dialog.Run()) //As felloe hooman beans we should help each other
        {
            case 0:
                menubar1.Show();
                SaveConf("menubar", "false");

                break;
            case 1:
                menubar1.Hide();
                SaveConf("menubar", "true");

                break;
            case 2: //Occey, dis worccs
                VBox vBox = new VBox();
                entry.Text = File.ReadAllText(working_dir + "conf/searchengine.conf");
                vBox.PackStart(entry, true, true, 3);
                HBox hBox = new HBox(false, 3);
                vBox.PackStart(hBox, true, true, 3);
                Button button_ok = new Button("OK");
                Button button_cancel = new Button("Cancel");
                hBox.PackStart(button_cancel, true, true, 3);
                hBox.PackStart(button_ok, true, true, 3);
                dialog_change.Add(vBox);
                Pixbuf iconw = new Pixbuf(working_dir + "icons/icon.png");
                dialog_change.Icon = iconw;
                dialog_change.ShowAll();
                button_ok.Clicked += this.OK;
                entry.Activated += this.OK;
                button_cancel.Clicked += this.Cancel;
                break;
            default:
                break;
        }
        dialog.Destroy();
        webView[notebook1.Page].GrabFocus();
    }
    //Ah yes, enslaved "FBI is going for your a$$"
    //I am not to be held responsible for some idiot trying to load an illegal 
    //Web page in my browser, since diss won't worcc for dat
    protected void EnableIncognito(object sender, EventArgs e)
    { //Dis seems to be deprecated. O FUCC
        if (incognito == true)
        {
            settings.EnablePrivateBrowsing = false;
            webView[notebook1.Page].Settings = settings;

        }
        else
        {
            settings.EnablePrivateBrowsing = true;
            webView[notebook1.Page].Settings = settings;

        }
    }
    void OK(object sender, EventArgs e)
    {
        searchEngine = entry.Text;
        File.WriteAllText(working_dir + "conf/searchengine.conf", entry.Text);
        dialog_change.Hide();
    }
    void Cancel(object sender, EventArgs e)
    {
        dialog_change.Hide();
    }
    protected void About(object sender, EventArgs e)
    {
        AboutDialog about = new AboutDialog();
        //If this can be called a web browser...
        about.ProgramName = "Greending Web Browser";
        //Fun facc: 私のお尻 means "My butt" in Japanese
        string[] authors = { "DisableGraphics", "私のお尻" };
        about.Authors = authors;
        about.Artists = authors;
        
        //Bicos dis program don't worcc correccly
        about.Version = "Version A_01";
        Pixbuf icon = new Pixbuf(working_dir + "icons/icon.png");
        about.Icon = icon;
        about.Logo = icon.ScaleSimple(32, 32, InterpType.Bilinear);
        about.LicenseType = License.Gpl30;
        about.Show();
        switch (about.Run())
        {
            case -4:
                about.Destroy();
                break;
        }
    }
    void GenerateTab(string url)
    {
        int newtab = notebook1.NPages;
        webView.Add(new WebView());
        Console.WriteLine(webView.Count);
        webView[newtab].Settings = settings;

        Label labl = new Label("Tab " + Convert.ToString(newtab + 1));
        notebook1.InsertPage(webView[newtab], labl, newtab);
        
        webView[newtab].Show();
        if (url != "default")
        {
            webView[newtab].LoadUri(url);
        }
        else
        {
            webView[newtab].LoadUri(initial_url);
        }
    }
    protected void NewTab(object sender, EventArgs e)
    {
        GenerateTab("default");
        webView[notebook1.Page].GrabFocus();

    }
    void RefreshTabNames()
    {
        if (notebook1.NPages > 0)
        {
            for (int i = 0; i < notebook1.NPages; i++)
            {
                notebook1.SetTabLabelText(notebook1.GetNthPage(i), "Tab " + Convert.ToString(i + 1));
            }
        }
    }
    protected void CloseTab(object sender, EventArgs e)
    {
        int pos = notebook1.Page;
        notebook1.RemovePage(notebook1.Page);
        webView.Remove(webView[pos]);
        RefreshTabNames();
        if(webView.Count > 0)
        {
            entry1.Text = webView[notebook1.Page].Uri;
        }
        else
        {
            entry1.Text = "";
        }
        webView[notebook1.Page].GrabFocus();

    }

    void SavePages()
    {
        File.Delete(working_dir + "conf/pages");
        for(int i = 0; i < notebook1.NPages; i++)
        {
            File.WriteAllText(working_dir + "conf/pages", webView[i].Uri);
        }
    }

    protected void OnChangeTab(object o, SwitchPageArgs args)
    {
        entry1.Text = webView[notebook1.Page].Uri;
    }

    void OnSaveAs()
    {
        Console.WriteLine("Test");
    }

    void FailedDownload()
    {
        Gtk.Window windoe = new Gtk.Window("Download failed");
        //Label label = new Label(download."");
    }

    void Loading()
    {

    }
}