using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace 踢足球
{
    public partial class Form1 : Form
    {
        string mapName = "map01.txt";
        Button[] btn = new Button[25];
        int[] map;
        int[] type;
        //每一位表示一种能力,0表示无能力
        const int typ_start = 1;//起点
        const int typ_end = 2;//终点
        const int typ_inc = 4;//每次停留加一
        const int typ_dec = 8;//每次停留减一
        const int typ_must = 16;//必过点
        const int typ_trans = 32;//传送门
        int pos;
        int power;
        string tips = "";
        int steps = 0;
        public Form1()
        {
            InitializeComponent();
        }
        private void ReadeXmlSerializer(Type type, string path)
        {
            string temp = "";
            object[] o = new object[51];
            //模拟读取数据库数据
            FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            temp = sr.ReadToEnd();

            //将xml转成object[]
            XmlSerializer xml = new XmlSerializer(type);
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(temp)))
            {
                try
                {
                    o = (object[])xml.Deserialize(ms);
                    for (int i = 0; i < 25; i++)
                    {
                        map[i] = int.Parse(o[i].ToString());
                        this.type[i] = int.Parse(o[25 + i].ToString());
                        ;
                    }
                    tips = o[50].ToString() ;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        /// <summary>
        /// 将数组转化成XML，
        /// </summary>
        /// <param name="type"></param>
        /// <param name="o"></param>
        /// <param name="path"></param>
        private static void WriteXmlSerializer(Type type, object[] o, string path)
        {
            string temp = "";
            XmlSerializer xml = new XmlSerializer(type);
            using (MemoryStream stream = new MemoryStream())
            {
                try
                {
                    xml.Serialize(stream, o);
                    temp = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                }
                catch (Exception)
                {
                    throw;
                }
                ///模拟数据库存储
                StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8);
                sw.Write(temp);
                sw.Close();
            }
        }
        void reStart()
        {

            try
            {
                object[] map_type = new object[50];
                ReadeXmlSerializer(map_type.GetType(), mapName);
            }
            catch
            {
                MessageBox.Show("文件不存在或者格式不对！！");
                return;
            }

            for (int i = 0; i < 25; i++)
            {
                if ((type[i] & typ_start) != 0)
                {
                    pos = i;
                    break;
                }
            }
            steps = 0;
            if(!tips.Equals("")) MessageBox.Show(tips);
            this.Text = "踢足球  by辣鸡土豆 " + mapName;
            print();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            btn[0] = button1;
            btn[1] = button2;
            btn[2] = button3;
            btn[3] = button4;
            btn[4] = button5;
            btn[5] = button6;
            btn[6] = button7;
            btn[7] = button8;
            btn[8] = button9;
            btn[9] = button10;
            btn[10] = button11;
            btn[11] = button12;
            btn[12] = button13;
            btn[13] = button14;
            btn[14] = button15;
            btn[15] = button16;
            btn[16] = button17;
            btn[17] = button18;
            btn[18] = button19;
            btn[19] = button20;
            btn[20] = button21;
            btn[21] = button22;
            btn[22] = button23;
            btn[23] = button24;
            btn[24] = button25;

            //map = new int[]
            //{
            //    4,1,4,3,1,
            //    1,3,2,1,3,
            //    2,1,1,3,1,
            //    1,2,2,1,2,
            //    3,2,1,1,2
            //};

            //type = new int[]
            //{
            //    0,2,typ_dec,0,0,
            //    0,0,typ_dec,0,0,
            //    0,0,typ_inc,0,0,
            //    0,0,typ_inc,1,0,
            //    0,0,typ_inc,0,0
            //};
            //map02



            // 1  ,  1  ,  e  ,  1  ,  1  ,
            // 1  ,  1G ,  1  ,  1  ,  1  ,
            // 1  ,  1  ,  1  ,  1  ,  1  ,
            // 1  ,  1* ,  1  ,  1s ,  1  ,
            // 1  ,  1  ,  1G ,  1  ,  1  ,



            map = new int[]
            {
                1,2,1,2,1,
                1,2,2,1,1,
                1,1,1,1,1,
                1,1,2,1,1,
                1,2,0,2,1
            };

            type = new int[]
            {
                0,0,2,0,0,
                0,typ_trans+typ_dec,0,0,0,
                0,0,0,0,0,
                0,0,0,1,0,
                0,0,typ_trans+typ_inc,typ_dec,0
            };
            for(int i = 0; i < 25; i++)
            {
                type[i] |= typ_must;
            }


            //tips = "带+的格子，你每次路过，它的值会+1，带-的格子，每次-1。数值为0就不能移动了~\n请合理利用加减规则~";
            tips = "带*的格子你必须路过，否则到达终点就算失败。带G的格子是传送门，你走到其中一个，会传送到另外一个。";
            object[] map_type = new object[51];
            for(int i = 0; i < 25; i++)
            {
                map_type[i] = map[i];
                map_type[i + 25] = type[i];
            }
            map_type[50]= tips;
            //WriteXmlSerializer(map_type.GetType(), map_type, mapName);
            reStart();
            //ReadeXmlSerializer(map_type.GetType(), "map.txt");
            //print();
        }
        void print()
        {
            label2.Text = "步数：" + steps.ToString();
            int ifend = 0;
            for(int i = 0; i < 5; i++)
            {
                for(int j = 0; j < 5; j++)
                {
                    
                    int index = i + j * 5;
                    btn[index].BackgroundImage = null;
                    btn[index].Text = map[index].ToString();
                    if((type[index] & typ_start) != 0)
                    {
                        btn[index].Text += " 起";
                    }
                    if ((type[index] & typ_end) != 0)
                    {
                        btn[index].Text = "终点";
                    }
                    if ((type[index] & typ_inc) != 0)
                    {
                        btn[index].Text += " +";
                    }
                    if ((type[index] & typ_dec) != 0)
                    {
                        btn[index].Text += " -";
                    }
                    if ((type[index] & typ_must) != 0)
                    {
                        btn[index].Text += " *";
                    }
                    if ((type[index] & typ_trans) != 0)
                    {
                        btn[index].Text += " G";
                    }
                    if (pos == index)
                    {
                        btn[index].BackgroundImage = Properties.Resources.ball_litte;
                        power = map[index];
                        label1.Text = "power:" + power.ToString();
                        
                    }
                    if ((type[index] & typ_end) != 0 && (pos == index))
                    {
                        ifend = 1;//win
                    }
                }
            }
            if (ifend == 1)
            {
                for(int i = 0; i < 25; i++)
                {
                    if((type[i] & typ_must) != 0)
                    {
                        MessageBox.Show("you lose!\n你还有任务点没有路过！必须路过全部*点才能来终点！");
                        reStart();
                        return;
                    }
                }
                MessageBox.Show("you win!");
                reStart();
            }
        }

        void leave()
        {
            steps += 1;

            if (power == 0)
            {
                MessageBox.Show("力量值为0，你不能移动");
            }
            if ((type[pos] & typ_dec) != 0)
            {
                if (map[pos] > 0) map[pos]--;
            }
            if ((type[pos] & typ_inc) != 0)
            {
                map[pos]++;
            }
        }
        void arrive()
        {

            if ((type[pos] & typ_trans) != 0)
            {
                for(int i = 0; i < 25; i++)
                {
                    if((type[i] & typ_trans) != 0 && pos != i)
                    {
                        pos = i;
                        break;
                    }
                }
            }

            if ((type[pos] & typ_must) != 0)
            {
                type[pos] -= typ_must;//路过就去除*
            }
        }
        private void bUp_Click(object sender, EventArgs e)
        {
            int i = pos - pos / 5 *5;
            int j = (pos - i) / 5;
            if (j - power >= 0)
            {
                leave();
                j -= power;
                pos = i + j * 5;
                arrive();
                print();
            }
            else
            {
                label1.Text = "你不能出界!!";
            }
        }

        private void bDown_Click(object sender, EventArgs e)
        {
            int i = pos - pos / 5 * 5;
            int j = (pos - i) / 5;
            if (j + power <= 4)
            {
                leave();
                j += power;
                pos = i + j * 5;
                arrive();
                print();
            }
            else
            {
                label1.Text = "你不能出界!!";
            }
        }

        private void bLeft_Click(object sender, EventArgs e)
        {
            int i = pos - pos / 5 * 5;
            int j = (pos - i) / 5;
            if (i - power >= 0)
            {
                leave();
                i -= power;
                pos = i + j * 5;
                arrive();
                print();
            }
            else
            {
                label1.Text = "你不能出界!!";
            }
        }

        private void bRight_Click(object sender, EventArgs e)
        {
            int i = pos - pos / 5 * 5;
            int j = (pos - i) / 5;
            if (i + power <= 4)
            {
                leave();
                i += power;
                pos = i + j * 5;
                arrive();
                print();
            }
            else
            {
                label1.Text = "你不能出界!!";
            }
        }

        private void button26_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.ShowDialog();
            string root = ofd.FileName;
            if (root.Equals("")) return;
            mapName = root;
            reStart();
        }

        private void bHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("这是个迷宫小游戏，通过上下左右来控制方向，每次的位移量取决于当前位置的数字。走到终点即可获胜。");
            if (!tips.Equals("")) MessageBox.Show(tips);
        }

        private void reSt_Click(object sender, EventArgs e)
        {
            reStart();
        }
    }
}
