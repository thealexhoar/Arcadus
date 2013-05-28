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

        public LevelGen(string url) {
            //while (true) {
                //Console.Write("Enter a URL: ");
                try {
                    //String uri = Console.ReadLine();
                    //if (uri.Equals("q")) Environment.Exit(1);
                    web = WebPageParser.FromUrl(url);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                    web = new WebPageParser();
                }
                this.GetLevel(web.GetTotalWebsiteElements());
                this.MakeDoors(5);
                this.title = web.GetWebpageTitle();
                this.colors = GetAllColors(web);
            //}
        }

        private List<Color> GetAllColors(WebPageParser wbpp) {
            List<Color> AllColors = new List<Color>();
            foreach (string hexcode in wbpp.GetAllWebpageColors()) {
                AllColors.Add(ParseColor(hexcode));
            }
            return AllColors;
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
            List<Tuple<int, int>> factors = this.getFactors(n);
            int rows = factors[factors.Count - 1].Item1;
            int columns = factors[factors.Count - 1].Item2;
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
            if (i >= Link_String.Count() || Link_String[i] == "") { i = 0; }
            dict.Add(t,Link_String[i]);
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
