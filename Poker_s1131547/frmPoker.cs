using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Poker_s1131547
{
    public partial class frmPoker : Form
    {
        PictureBox[] pic = new PictureBox[5];
        int[] allPoker = new int[52];
        int[] playerPoker = new int[5];
        public frmPoker()
        {
            InitializeComponent();
            InitializePoker();
        }
        private void InitializePoker()
        {
            // 動態產生5張牌
            for (int i = 0; i < pic.Length; i++)
            {
                pic[i] = new PictureBox();
                pic[i].Image = GetImage("back");
                pic[i].Name = "pic" + i;
                pic[i].SizeMode = PictureBoxSizeMode.AutoSize;
                pic[i].Top = 30;
                pic[i].Left = 10 + ((pic[i].Width + 10) * i);
                pic[i].Enabled = false;
                pic[i].Tag = "牌背";
                pic[i].Visible = true;
                this.grpPoker.Controls.Add(pic[i]);
                pic[i].Click += Pic_Click;
            }
        }
        /// <summary>
        /// 顯示五張撲克牌到桌面上
        /// </summary>
        private void ShowCards()
        {
            for (int i = 0; i < playerPoker.Length; i++)
            {
                pic[i].Image = this.GetImage($"pic{playerPoker[i] + 1}");
            }
        }


        private Image GetImage(string name)
        {
            return Properties.Resources.ResourceManager.GetObject(name) as Image;
        }
        private Image GetImage(int num)
        {
            return GetImage($"pic{num}");
        }

        private void Pic_Click(object sender, EventArgs e)
        {
            PictureBox pic = sender as PictureBox;
            int index = int.Parse(pic.Name.Replace("pic", ""));
            int cardNumer = playerPoker[index] + 1;
            MessageBox.Show($"牌編號{cardNumer}");

            if (pic.Tag.ToString() == "back")
            {
                pic.Tag = "front";
                pic.Image = GetImage(cardNumer);
            }
            else
            {
                pic.Tag = "back";
                pic.Image = GetImage("back");
            }
        }

        private async void btnDealCard_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                pic[i].Image = GetImage("back");
            }
            // 初始化52張牌
            for (int i = 0; i < allPoker.Length; i++)
            {
                allPoker[i] = i;
            }
            // 洗牌
            this.Shuffle();

            // 暫停500ms
            await Task.Delay(500);

            // 發牌
            for (int i = 0; i < playerPoker.Length; i++)
            {
                playerPoker[i] = allPoker[i];
            }
            // 同花大順
            //playerPoker[0] = 51;
            //playerPoker[1] = 47;
            //playerPoker[2] = 43;
            //playerPoker[3] = 39;
            //playerPoker[4] = 3;
            // 同花順
            //playerPoker[0] = 37;
            //playerPoker[1] = 33;
            //playerPoker[2] = 29;
            //playerPoker[3] = 25;
            //playerPoker[4] = 21;
            // 同花
            //playerPoker[0] = 50;
            //playerPoker[1] = 38;
            //playerPoker[2] = 34;
            //playerPoker[3] = 22;
            //playerPoker[4] = 18;
            // 鐵支
            //playerPoker[0] = 48;
            //playerPoker[1] = 39;
            //playerPoker[2] = 38;
            //playerPoker[3] = 37;
            //playerPoker[4] = 36;
            // 葫蘆
            //playerPoker[0] = 30;
            //playerPoker[1] = 29;
            //playerPoker[2] = 6;
            //playerPoker[3] = 5;
            //playerPoker[4] = 4;
            // 三條
            //playerPoker[0] = 48;
            //playerPoker[1] = 39;
            //playerPoker[2] = 15;
            //playerPoker[3] = 14;
            //playerPoker[4] = 13;

            this.ShowCards();

            for (int i = 0; i < pic.Length; i++)
            {
                pic[i].Enabled = true;
                pic[i].Tag = "front";
            }
            btnChangeCard.Enabled = true;
            btnDealCard.Enabled = false;
            this.lblResult.Text = string.Empty;
        }
        private void Shuffle()
        {
            Random rand = new Random();
            for (int i = 0; i < allPoker.Length; i++)
            {
                int r = rand.Next(allPoker.Length);
                int temp = allPoker[r];
                allPoker[r] = allPoker[0];
                allPoker[0] = temp;
            }
        }
        private void btnChangeCard_Click(object sender, EventArgs e)
        {
            int cardIndex = 5;
            for (int i = 0; i < pic.Length; i++)
            {
                if (pic[i].Tag.ToString() == "back")
                {
                    playerPoker[i] = allPoker[cardIndex];
                    pic[i].Image = GetImage(playerPoker[i] + 1);
                    pic[i].Tag = "front";
                    cardIndex++;
                }
            }
            // 禁用所有牌的點擊事件
            for (int i = 0; i < pic.Length; i++)
            {
                pic[i].Enabled = false;
            }
            this.btnChangeCard.Enabled = false;
            this.btnCheck.Enabled = true;
        }
        private void btnCheck_Click(object sender, EventArgs e)
        {
            string[] colorList = { "梅花", "方塊", "愛心", "黑桃" };
            string[] pointList = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

            // 計錄目前五張撲克牌的花色和點數的陣列
            int[] pokerColor = new int[5];
            int[] pokerPoint = new int[5];

            // 將每張牌的顏色和點數分別存入 pokerColor 和 pokerPoint 陣列
            for (int i = 0; i < playerPoker.Length; i++)
            {
                pokerColor[i] = playerPoker[i] % 4;
                pokerPoint[i] = playerPoker[i] / 4;
            }

            /*string result = "玩家: ";
            for(int i = 0; i < playerPoker.Length; i++)
            {
                int iColor = pokerColor[i];
                int iPoint = pokerPoint[i];
                result += $"{colorList[iColor]}{pointList[iPoint]}";
            }
            this.lblResult.Text = result;*/
            int[] colorCount = new int[4];
            int[] pointCount = new int[13];
            for (int i = 0; i < 5; i++)
            {
                int color = pokerColor[i];
                int point = pokerPoint[i];
                colorCount[color]++;
                pointCount[point]++;
            }
            Array.Sort(colorCount, colorList);
            Array.Reverse(colorCount);
            Array.Reverse(colorList);
            Array.Sort(pointCount, pointList);
            Array.Reverse(pointCount);
            Array.Reverse(pointList);

            // 判斷是否為同花
            bool isFlush = (colorCount[0] == 5);
            // 判斷是否為五張單張
            bool isSingle = (pointCount[0] == 1 && pointCount[1] == 1 && pointCount[2] == 1 &&
            pointCount[3] == 1 && pointCount[4] == 1);
            // 判斷是否為差四
            bool isDiffFout = (pokerPoint.Max() - pokerPoint.Min() == 4);
            // 判斷是否為大順
            bool isRoyal = pokerPoint.Contains(0) && pokerPoint.Contains(9) &&
            pokerPoint.Contains(10) && pokerPoint.Contains(11) && pokerPoint.Contains(12);
            // 判斷是否為同花大順
            bool isRoyalisFlush = isFlush && isRoyal;
            // 判斷是否為同花順
            bool isStraightFlush = isFlush && isSingle && isDiffFout;
            // 判斷是否為順子
            bool isStraight = isSingle && (isDiffFout || isRoyal);
            // 判斷是否為鐵支
            bool isFourOfAKind = (pointCount[0] == 4);
            // 判斷是否為葫蘆
            bool isFullHouse = (pointCount[0] == 3 && pointCount[1] == 2);
            // 判斷是否為三條
            bool isThreeOfAKind = (pointCount[0] == 3 && pointCount[1] == 1);
            // 判斷是否為兩對
            bool isTwoPair = (pointCount[0] == 2 && pointCount[1] == 2);
            // 判斷是否為一對
            bool isOnePair = (pointCount[0] == 2 && pointCount[1] == 1);

            string result = "";
            int odds = 0;
            if (isRoyalisFlush)
            {
                result = $"{colorList[0]} 同花大順";
                odds = 250;
            }
            else if (isStraightFlush)
            {
                result = $"{colorList[0]} 同花順";
                odds = 50;
            }
            else if (isStraight)
            {
                result = "順子";
                odds = 25;
            }
            else if (isFourOfAKind)
            {
                result = $"{pointList[0]} 鐵支";
                odds = 9;
            }
            else if (isFullHouse)
            {
                result = $"{pointList[0]}三張{pointList[1]}兩張 葫蘆";
                odds = 6;
            }
            else if (isFlush)
            {
                result = $"{colorList[0]} 同花";
                odds = 4;
            }
            else if (isThreeOfAKind)
            {
                result = $"{pointList[0]} 三條";
                odds = 3;
            }
            else if (isTwoPair)
            {
                result = $"{pointList[0]},{pointList[1]} 兩對";
                odds = 2;
            }
            else if (isOnePair)
            {
                result = $"{pointList[0]} 一對";
                odds = 1;
            }
            else
            {
                result = "雜牌";
                odds = 0;
            }
            lblResult.Text = result;
            long winnings = currentBet * odds;
            totalMoney += winnings;
            txtTotalMoney.Text = totalMoney.ToString();

            if (odds > 0)
            {
                MessageBox.Show($"恭喜獲得 {result}！\n贏得獎金：{winnings} 元", "結算", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("未中獎，請再接再厲！", "結算", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            btnChangeCard.Enabled = false;
            btnCheck.Enabled = false;
            btnDealCard.Enabled = false;

            txtBetAmount.Enabled = true; // 解鎖輸入框
            btnBet.Enabled = true;       // 解鎖下注按鈕
            txtBetAmount.Clear();        // 清空下注金額
            currentBet = 0;              // 歸零當前下注額
        }

        private void frmPoker_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (btnDealCard.Enabled == false)
            {
                bool isCheatKey = true;
                switch (e.KeyChar)
                {
                    case 'q': // q鍵
                              // 同花大順
                        playerPoker[0] = 51;
                        playerPoker[1] = 47;
                        playerPoker[2] = 43;
                        playerPoker[3] = 39;
                        playerPoker[4] = 3;
                        break;
                    case 'w': // w鍵
                              // 同花順
                        playerPoker[0] = 37;
                        playerPoker[1] = 33;
                        playerPoker[2] = 29;
                        playerPoker[3] = 25;
                        playerPoker[4] = 21;
                        break;
                    case 'e': // e鍵
                              // 同花
                        playerPoker[0] = 50;
                        playerPoker[1] = 38;
                        playerPoker[2] = 34;
                        playerPoker[3] = 22;
                        playerPoker[4] = 18;
                        break;
                    case 'r': // r鍵
                              // 鐵支
                        playerPoker[0] = 48;
                        playerPoker[1] = 39;
                        playerPoker[2] = 38;
                        playerPoker[3] = 37;
                        playerPoker[4] = 36;
                        break;
                    case 't':  // t鍵
                               // 葫蘆
                        playerPoker[0] = 30;
                        playerPoker[1] = 29;
                        playerPoker[2] = 6;
                        playerPoker[3] = 5;
                        playerPoker[4] = 4;
                        break;
                    case 'y':  // y鍵
                               // 三條
                        playerPoker[0] = 48;
                        playerPoker[1] = 39;
                        playerPoker[2] = 15;
                        playerPoker[3] = 14;
                        playerPoker[4] = 13;
                        break;

                    default:
                        // 如果按下的鍵不是上面任何一個 (例如輸入數字)，就把標記設為 false
                        isCheatKey = false;
                        break;
                }
                if (isCheatKey)
                {
                    // 顯示五張撲克牌到桌面上
                    ShowCards();
                }
            }
        }
        private long totalMoney = 1000000;
        private long currentBet = 0;
        private void btnBet_Click(object sender, EventArgs e)
        {
            // 1. 防呆：檢查是否完全沒輸入
            if (string.IsNullOrWhiteSpace(txtBetAmount.Text))
            {
                MessageBox.Show("請輸入押注金額！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // 中斷執行，讓玩家重新輸入
            }

            // 2. 防呆：檢查輸入的是不是「整數數字」
            if (!long.TryParse(txtBetAmount.Text, out currentBet))
            {
                MessageBox.Show("押注金額必須是正整數！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtBetAmount.Clear(); // 清空錯誤的輸入
                return;
            }

            // 3. 防呆：檢查押注金額是否合理 (必須大於 0)
            if (currentBet <= 0)
            {
                MessageBox.Show("押注金額必須大於 0！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4. 防呆：檢查餘額是否足夠
            if (currentBet > totalMoney)
            {
                MessageBox.Show($"總資金不足！您目前只剩下 {totalMoney} 元。", "餘額不足", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 5. 扣除總資金
            totalMoney -= currentBet;

            // 6. 更新介面上的總資金顯示
            txtTotalMoney.Text = totalMoney.ToString();

            // 7. 鎖定下注欄位與按鈕，防止在開獎前重複下注或偷改金額
            txtBetAmount.Enabled = false;
            btnBet.Enabled = false;
            btnDealCard.Enabled = true;

            // 提示玩家下注成功，可以準備發牌了
            MessageBox.Show($"成功下注 {currentBet} 元！請開始發牌。", "下注成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmPoker_Load(object sender, EventArgs e)
        {
            txtTotalMoney.Text = totalMoney.ToString();
            // 遊戲剛開始時，必須先下注才能發牌
            btnDealCard.Enabled = false;
            btnChangeCard.Enabled = false;
            btnCheck.Enabled = false;
        }
    }
}
