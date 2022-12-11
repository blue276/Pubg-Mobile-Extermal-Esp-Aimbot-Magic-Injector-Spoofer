susing System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Vector3 = SharpDX.Vector3;
using Vector2 = SharpDX.Vector2;

namespace RadarTut
{
    public partial class frmMain : Form
    {
        #region STRUCTS
        private struct Player
        {
            public int iHealth;
            public int iTeam;
            public int Index;
            public Vector3 vec3Position;
            public Vector3 vec3ViewAngles;

            public Player(int index, int health, int team, Vector3 position, Vector3 viewAngles)
            {
                Index = index;
                iHealth = health;
                iTeam = team;
                vec3Position = position;
                vec3ViewAngles = viewAngles;
            }
        }
        #endregion
        
        #region VARIABLES
        private Player localPlayer;
        private Player[] playerList;
        private Random random;
        private Vector2 radarCenter;
        private static int RADAR_RADIUS = 150;
        private static int PLAYER_BOX_SIZE = 8;
        private static int PLAYER_FOV_SIZE = 8;
        #endregion

        #region CONSTRUCTOR
        public frmMain()
        {
            InitializeComponent();
            this.ClientSize = new Size(RADAR_RADIUS * 2, RADAR_RADIUS * 2 + settingsBox.Height);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.DoubleBuffered = true;
            this.redrawTimer.Interval = (int)(1000.0 / 60.0);

            radarCenter = new Vector2(RADAR_RADIUS, RADAR_RADIUS);
            random = new Random();
            playerList = new Player[16];
            CreatePlayers();
        }
        #endregion

        #region PLAYERS
        private void CreatePlayers()
        {
            int maxDistance = 150, center = 150;
            for (int i = 0; i < playerList.Length; i++)
            {
                random = new Random(Environment.TickCount + random.Next(int.MinValue, int.MaxValue));
                playerList[i] = new Player(
                    i,
                    100,
                    (i % 2 == 0 ? 2 : 3),
                    new Vector3(
                        center + random.Next(-maxDistance, maxDistance),
                        center + random.Next(-maxDistance, maxDistance),
                        center + random.Next(-maxDistance, maxDistance)
                    ),
                    new Vector3(
                        random.Next(-180, 180),
                        random.Next(-180, 180),
                        random.Next(-180, 180)
                    )
                );
            }
            localPlayer = playerList[0];
        }
        private void MovePlayers()
        {
            for (int i = 0; i < playerList.Length; i++)
            {
                playerList[i].vec3Position += Vector3.One;
            }
            localPlayer = playerList[0];
        }
        #endregion

        #region EVENTS
        private void redrawTimer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
        }
        
        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
            //Update first
            MovePlayers();
            
            //Setup AA and clear form
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            e.Graphics.Clear(Color.DimGray);
            e.Graphics.FillEllipse(Brushes.DarkGray, 0, 0, RADAR_RADIUS * 2 - 1, RADAR_RADIUS * 2 - 1);

            //Draw players
            Vector2 myPos = Vector3ToVector2(localPlayer.vec3Position);
            for (int i = 0; i < playerList.Length; i++)
            {
                if (playerList[i].iHealth <= 0)
                    continue;
                //Obtain vec2 of player
                Vector2 screenPos = Vector3ToVector2(playerList[i].vec3Position);
                //Reduce it to a direction-vector
                screenPos = myPos - screenPos;
                //Scale it and clip it if it is out of our radar-bounds
                float distance = screenPos.Length() * (0.02f * tbrZoom.Value);
                distance = Math.Min(distance, RADAR_RADIUS);
                screenPos.Normalize();
                screenPos *= distance;
                //Apply it to the center of the radar
                screenPos += radarCenter;
                //Rotate it according to our yaw
                screenPos = RotatePoint(screenPos, radarCenter, localPlayer.vec3ViewAngles.Y - 90);
                //Draw the player
                DrawPlayer(playerList[i], screenPos, e.Graphics);
            }
        }

        private void tbrYaw_ValueChanged(object sender, EventArgs e)
        {
            playerList[0].vec3ViewAngles.Y = tbrYaw.Value - 180f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreatePlayers();
        }
        #endregion

        #region HELPER-METHODS
        private void DrawPlayer(Player player, Vector2 pos, Graphics graphics)
        {
            Brush color = (player.iTeam == 2 ? Brushes.Blue : Brushes.Red);
            if (player.Index == localPlayer.Index)
                color = Brushes.Gold;
            //Draw ellipse
            graphics.FillEllipse(
                color,
                pos.X - PLAYER_BOX_SIZE / 2f, pos.Y - PLAYER_BOX_SIZE / 2f, PLAYER_BOX_SIZE, PLAYER_BOX_SIZE
            );
            //Prepare FOV
            Vector2[] fov = new Vector2[3];
            fov[0] = pos;
            fov[1] = pos - Vector2.UnitY * PLAYER_FOV_SIZE + Vector2.UnitX * PLAYER_FOV_SIZE;
            fov[2] = pos - Vector2.UnitY * PLAYER_FOV_SIZE - Vector2.UnitX * PLAYER_FOV_SIZE;
            //Rotate FOV according to this player's and localPlayer's yaw
            fov[1] = RotatePoint(fov[1], pos, localPlayer.vec3ViewAngles.Y - player.vec3ViewAngles.Y);
            fov[2] = RotatePoint(fov[2], pos, localPlayer.vec3ViewAngles.Y - player.vec3ViewAngles.Y);
            //Draw FOV
            graphics.FillPolygon(
                color,
                Vector2ToPointF(fov)
            );
        }
        private static Vector2 Vector3ToVector2(Vector3 vec3) { return new Vector2(vec3.X, vec3.Y); }
        private static PointF[] Vector2ToPointF(params Vector2[] vecs2) {
            PointF[] pts = new PointF[vecs2.Length];
            for (int i = 0; i < vecs2.Length; i++) pts[i] = new PointF(vecs2[i].X, vecs2[i].Y);
            return pts;
        }
        private static Vector2 RotatePoint(Vector2 pointToRotate, Vector2 centerPoint, float angle, bool angleInRadians = false)
        {
            if(!angleInRadians)
                angle = (float)(angle * (Math.PI / 180f));
            float cosTheta = (float)Math.Cos(angle);
            float sinTheta = (float)Math.Sin(angle);
            Vector2 returnVec = new Vector2(
                cosTheta * (pointToRotate.X - centerPoint.X) - sinTheta * (pointToRotate.Y - centerPoint.Y),
                sinTheta * (pointToRotate.X - centerPoint.X) + cosTheta * (pointToRotate.Y - centerPoint.Y)
            );
            returnVec += centerPoint;
            return returnVec;
        }
        private static float DegToRad(float deg) { return (float)(deg * (Math.PI / 180f)); }
        private static float RadToDeg(float deg) { return (float)(deg * (180f / Math.PI)); }
        #endregion
    }
}
s