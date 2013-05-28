using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using HtmlAgilityPack;
//Nikko R.
namespace GetLevel {
    public class LevelGen {

        WebPageParser web;
        public int[,] LevelGrid;
        public string title;
        public List<Color> colors;
        public Color primaryColor;

        public LevelGen(string url) {
            //while (true) {
                Console.WriteLine("Creating web page parser");
                try {
                    //String uri = Console.ReadLine();
                    //if (uri.Equals("q")) Environment.Exit(1);
                    web = WebPageParser.FromUrl(url);
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                    web = new WebPageParser();
                }
                Console.WriteLine("Web page parser created successfully");
                this.GetLevel(web.GetTotalWebsiteElements());
                this.MakeDoors(Math.Min((LevelGrid.GetLength(0) + LevelGrid.GetLength(1)) / 2, web.GetAllLinks().Count));
                this.title = web.GetWebpageTitle();
                this.colors = GetAllColors(web);
                this.primaryColor = getMostColorful(this.colors);
            //}
        }

        private List<Color> GetAllColors(WebPageParser wbpp) {
            List<Color> AllColors = new List<Color>();
            foreach (string hexcode in wbpp.GetAllWebpageColors()) {
                AllColors.Add(ParseColor(hexcode));
            }
            AllColors.Sort(delegate(Color a, Color b) {
                if (GetColorfulnessIndex(a) > GetColorfulnessIndex(b)) { return 1; }
                else if (GetColorfulnessIndex(a) == GetColorfulnessIndex(b)) { return 0; }
                else return -1;
            });
            return AllColors;
        }

        private Color getMostColorful(List<Color> Colors) {
            Color bestColor = Colors[0];
            foreach (Color color in Colors) {
                if (GetColorfulnessIndex(color) > GetColorfulnessIndex(bestColor)) { bestColor = color; }
            }
            return bestColor;
        }
        private int GetColorfulnessIndex(Color color) {
            return Math.Abs(color.R - color.G) + Math.Abs(color.G - color.B) + Math.Abs(color.R - color.B);
        }

        private static Color ParseColor(string hexcode) {
            //hexcode = hexcode.Substring(1);
            string hexR, hexG, hexB;
            if (hexcode.Count() != 4 && hexcode.Count() != 7) { return Color.Black; }
            if (hexcode.Count() == 4) {
                hexR = hexcode[1] + "" + hexcode[1];
                hexG = hexcode[2] + "" + hexcode[2];
                hexB = hexcode[3] + "" + hexcode[3];
            }
            else {
                hexR = hexcode.Substring(1, 2);
                hexG = hexcode.Substring(3, 2);
                hexB = hexcode.Substring(5, 2);
            }
            int r, g, b;
            r = Convert.ToInt32(hexR, 16);
            g = Convert.ToInt32(hexG, 16);
            b = Convert.ToInt32(hexB, 16);
            return new Color(r, g, b);
        }

        public void GetLevel(int n) {
            if (n < 12) { n = 12; }
            List<Tuple<int, int>> factors = this.getFactors(n);
            int rows = 0, columns = 0;
            while (rows <= 3 || columns <= 3) {
                factors = this.getFactors(n);
                rows = factors[factors.Count - 1].Item1;
                columns = factors[factors.Count - 1].Item2;
                n++;
            }
            LevelGrid = new int[rows, columns];
            
            //Make the edge of the map all walls
            this.makeWalls();
        }
        private List<Tuple<int, int>> getFactors(int n) {
            List<Tuple<int, int>> FactorList = new List<Tuple<int, int>>();
            for (int i = 1; i <= n / 2; i++) {
                if (n % i == 0) {
                    if (!FactorList.Contains(new Tuple<int, int>(n / i, i))) {
                        FactorList.Add(new Tuple<int, int>(i, n / i));
                    }
                }
            }
            return FactorList;
        }
        private void makeWalls() {
            //Turns the edges of the level into walls
            for (int r = 0; r <= LevelGrid.GetLength(0) - 1; r++) {
                for (int c = 0; c <= LevelGrid.GetLength(1) - 1; c++) {
                    if (r == 0) {
                        LevelGrid[r, c] = 1;
                    }
                    else if (r == LevelGrid.GetLength(0) - 1) {
                        LevelGrid[r, c] = 1;
                    }
                    if (c == 0) {
                        LevelGrid[r, c] = 1;
                    }
                    else if (c == LevelGrid.GetLength(1) - 1) {
                        LevelGrid[r, c]=1;
                    }
                }
            }
        }

        public Dictionary<Tuple<int, int>, String> dict = new Dictionary<Tuple<int, int>, string>();
        public void MakeDoors(int doors) {
            List<Tuple<int,int>> EdgeList=this.GetEdge();
            int Space = EdgeList.Count / doors;
            int count=0;
            int i=0;
            while (count != doors) {
                i = count * Space;
                while (isCorner(EdgeList[i].Item1, EdgeList[i].Item2)) {
                    i++;
                }
                Console.WriteLine("");
                LevelGrid[EdgeList[i].Item1, EdgeList[i].Item2] = 2;
                LevelGrid[EdgeList[i + 1].Item1, EdgeList[i + 1].Item2] = 2;
                getDoorDict(EdgeList[i],count);
                getDoorDict(EdgeList[i+1],count);
                count++;
            }
        }
        private List<Tuple<int,int>> GetEdge(){
            List<Tuple<int,int>>x =new List<Tuple<int,int>>();
            int r = 0;
            int c = 0;

            for (c = 0; c <= LevelGrid.GetLength(1) - 1; c++) {
                x.Add(new Tuple<int, int>(r, c));
            }

            c = LevelGrid.GetLength(1) - 1;
            for (r = 0; r <= LevelGrid.GetLength(0) - 1; r++) {
                x.Add(new Tuple<int, int>(r, c));
            }

            r = LevelGrid.GetLength(0) - 1;
            for (c = LevelGrid.GetLength(1)-1; c >= 0; c--) {
                x.Add(new Tuple<int, int>(r, c));
            }

            c = 0;
            for (r = LevelGrid.GetLength(0)-1; r >= 0 ; r--) {
                x.Add(new Tuple<int, int>(r, c));
            }


            
            return x;
        }
        private bool isCorner(int r, int c) {
            if (r == 0 && c == 0) {
                return true;
            }
            else if (r == LevelGrid.GetLength(0)-1 && c == 0) {
                return true;
            }
            else if (r == 0 && c == LevelGrid.GetLength(1)-1) {
                return true;
            }
            else if (r == LevelGrid.GetLength(0)-1 && c == LevelGrid.GetLength(1)-1) {
                return true;
            }
            return false;
        }
        private void getDoorDict(Tuple<int,int> t,int i) {
            List<string> Link_String = web.GetAllLinks();
            Random rand = new Random();
            i = rand.Next(0, Link_String.Count);
            while (i >= Link_String.Count || Link_String[i] == "" || Link_String[i][0] == '#' || Link_String[i].IndexOf("javascipt") > 0) {
                i = rand.Next(0, Link_String.Count);
            }
            if (!dict.Keys.Contains(t)) { dict.Add(t,Link_String[i]); }
        }


        public void PrintLevel() {
            for (int r = 0; r <= LevelGrid.GetLength(0) - 1; r++) {
                for (int c = 0; c <= LevelGrid.GetLength(1) - 1; c++) {
                    Console.Write(LevelGrid[r,c]+" ");
                }
                Console.WriteLine("");
                Console.WriteLine("");
            }
        }
    }
}
