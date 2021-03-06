﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HslCommunication.Profinet;
using HslCommunication;
using HslCommunication.ModBus;
using System.Threading;

namespace ModbusTcpServer
{
    public partial class FormModbus : Form
    {
        public FormModbus()
        {
            InitializeComponent( );
        }

        

        private void linkLabel1_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            try
            {
                System.Diagnostics.Process.Start( linkLabel1.Text );
            }
            catch (Exception ex) 
            {
                MessageBox.Show( ex.Message );
            }
        }

        private void FormSiemens_Load( object sender, EventArgs e )
        {
            panel2.Enabled = false;
        }

        private void FormSiemens_FormClosing( object sender, FormClosingEventArgs e )
        {
            
        }

        /// <summary>
        /// 统一的读取结果的数据解析，显示
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <param name="address"></param>
        /// <param name="textBox"></param>
        private void readResultRender<T>( T result, string address, TextBox textBox )
        {
            textBox.AppendText( DateTime.Now.ToString( "[HH:mm:ss] " ) + $"[{address}] {result}{Environment.NewLine}" );
        }

        /// <summary>
        /// 统一的数据写入的结果显示
        /// </summary>
        /// <param name="result"></param>
        /// <param name="address"></param>
        private void writeResultRender( string address )
        {
            MessageBox.Show( DateTime.Now.ToString( "[HH:mm:ss] " ) + $"[{address}] 写入成功" );
        }


        #region Server Start

   

        private void button1_Click( object sender, EventArgs e )
        {
            if(!int.TryParse(textBox2.Text,out int port))
            {
                MessageBox.Show( "端口输入不正确！" );
                return;
            }



            try
            {

                busTcpServer = new HslCommunication.ModBus.ModbusTcpServer( );
                busTcpServer.LogNet = new HslCommunication.LogNet.LogNetSingle( "logs.txt" );
                busTcpServer.LogNet.BeforeSaveToFile += LogNet_BeforeSaveToFile;
                busTcpServer.OnDataReceived += BusTcpServer_OnDataReceived;
                busTcpServer.ServerStart( port );

                button1.Enabled = false;
                panel2.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }

        private void BusTcpServer_OnDataReceived( byte[] modbus )
        {
            textBox1.AppendText( "接收数据：" + HslCommunication.BasicFramework.SoftBasic.ByteToHexString(modbus) + Environment.NewLine );
        }

        private void LogNet_BeforeSaveToFile( object sender, HslCommunication.LogNet.HslEventArgs e )
        {
            if(InvokeRequired)
            {
                BeginInvoke( new Action<object, HslCommunication.LogNet.HslEventArgs>( LogNet_BeforeSaveToFile ), sender, e );
                return;
            }

            textBox1.AppendText( e.HslMessage.ToString( ) + Environment.NewLine );
        }
        
        private HslCommunication.ModBus.ModbusTcpServer busTcpServer;
        

        #endregion

        #region 单数据读取测试


        private void button_read_bool_Click( object sender, EventArgs e )
        {
            // 读取bool变量
            readResultRender( busTcpServer.ReadCoil( ushort.Parse( textBox3.Text ) ), textBox3.Text, textBox4 );
        }

        private void button_read_short_Click( object sender, EventArgs e )
        {
            // 读取short变量
            readResultRender( busTcpServer.ReadInt16( ushort.Parse( textBox3.Text ) ), textBox3.Text, textBox4 );
        }

        private void button_read_ushort_Click( object sender, EventArgs e )
        {
            // 读取ushort变量
            readResultRender( busTcpServer.ReadUInt16( ushort.Parse( textBox3.Text ) ), textBox3.Text, textBox4 );
        }

        private void button_read_int_Click( object sender, EventArgs e )
        {
            // 读取int变量
            readResultRender( busTcpServer.ReadInt32( ushort.Parse( textBox3.Text ) ), textBox3.Text, textBox4 );
        }
        private void button_read_uint_Click( object sender, EventArgs e )
        {
            // 读取uint变量
            readResultRender( busTcpServer.ReadUInt32( ushort.Parse( textBox3.Text ) ), textBox3.Text, textBox4 );
        }
        private void button_read_long_Click( object sender, EventArgs e )
        {
            // 读取long变量
            readResultRender( busTcpServer.ReadInt64( ushort.Parse( textBox3.Text ) ), textBox3.Text, textBox4 );
        }

        private void button_read_ulong_Click( object sender, EventArgs e )
        {
            // 读取ulong变量
            readResultRender( busTcpServer.ReadUInt64( ushort.Parse( textBox3.Text ) ), textBox3.Text, textBox4 );
        }

        private void button_read_float_Click( object sender, EventArgs e )
        {
            // 读取float变量
            readResultRender( busTcpServer.ReadFloat( ushort.Parse( textBox3.Text ) ), textBox3.Text, textBox4 );
        }

        private void button_read_double_Click( object sender, EventArgs e )
        {
            // 读取double变量
            readResultRender( busTcpServer.ReadDouble( ushort.Parse( textBox3.Text ) ), textBox3.Text, textBox4 );
        }

        private void button_read_string_Click( object sender, EventArgs e )
        {
            // 读取字符串
            readResultRender( busTcpServer.ReadString( ushort.Parse( textBox3.Text ), ushort.Parse( textBox5.Text ) ), textBox3.Text, textBox4 );
        }


        #endregion

        #region 单数据写入测试


        private void button24_Click( object sender, EventArgs e )
        {
            // bool写入
            try
            {
                busTcpServer.WriteCoil( ushort.Parse( textBox8.Text ), bool.Parse( textBox7.Text ) );
                writeResultRender( textBox8.Text );
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }

        private void button22_Click( object sender, EventArgs e )
        {
            // short写入
            try
            {
                busTcpServer.Write( ushort.Parse( textBox8.Text ), short.Parse( textBox7.Text ) );
                writeResultRender( textBox8.Text );
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }

        private void button21_Click( object sender, EventArgs e )
        {
            // ushort写入
            try
            {
                busTcpServer.Write( ushort.Parse( textBox8.Text ), ushort.Parse( textBox7.Text ) );
                writeResultRender( textBox8.Text );
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }


        private void button20_Click( object sender, EventArgs e )
        {
            // int写入
            try
            {
                busTcpServer.Write( ushort.Parse( textBox8.Text ), int.Parse( textBox7.Text ) );
                writeResultRender(  textBox8.Text );
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }

        private void button19_Click( object sender, EventArgs e )
        {
            // uint写入
            try
            {
                busTcpServer.Write( ushort.Parse( textBox8.Text ), uint.Parse( textBox7.Text ) );
                writeResultRender( textBox8.Text );
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }

        private void button18_Click( object sender, EventArgs e )
        {
            // long写入
            try
            {
                busTcpServer.Write( ushort.Parse( textBox8.Text ), long.Parse( textBox7.Text ) );
                writeResultRender(  textBox8.Text );
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }

        private void button17_Click( object sender, EventArgs e )
        {
            // ulong写入
            try
            {
                busTcpServer.Write( ushort.Parse( textBox8.Text ), ulong.Parse( textBox7.Text ) );
                writeResultRender(  textBox8.Text );
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }

        private void button16_Click( object sender, EventArgs e )
        {
            // float写入
            try
            {
                busTcpServer.Write( ushort.Parse( textBox8.Text ), float.Parse( textBox7.Text ) );
                writeResultRender(  textBox8.Text );
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }

        private void button15_Click( object sender, EventArgs e )
        {
            // double写入
            try
            {
                busTcpServer.Write( ushort.Parse( textBox8.Text ), double.Parse( textBox7.Text ) );
                writeResultRender(  textBox8.Text );
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }


        private void button14_Click( object sender, EventArgs e )
        {
            // string写入
            try
            {
                busTcpServer.Write( ushort.Parse( textBox8.Text ), textBox7.Text );
                writeResultRender(  textBox8.Text );
            }
            catch (Exception ex)
            {
                MessageBox.Show( ex.Message );
            }
        }



        #endregion

        private void button2_Click( object sender, EventArgs e )
        {
            // 点击数据监视
            ModBusMonitorAddress monitorAddress = new ModBusMonitorAddress( );
            monitorAddress.Address = ushort.Parse( textBox6.Text );
            monitorAddress.OnChange += MonitorAddress_OnChange;
            monitorAddress.OnWrite += MonitorAddress_OnWrite;
            busTcpServer.AddSubcription( monitorAddress );
            button2.Enabled = false;
        }

        private void MonitorAddress_OnWrite( ModBusMonitorAddress monitor, short value )
        {
            // 当有客户端写入时就触发
        }

        private void MonitorAddress_OnChange( ModBusMonitorAddress monitor, short befor, short after )
        {
            // 当该地址的值更改的时候触发
            if(InvokeRequired)
            {
                BeginInvoke( new Action<ModBusMonitorAddress, short, short>( MonitorAddress_OnChange ), monitor, befor, after );
                return;
            }

            textBox9.Text = after.ToString( );

            label11.Text = "写入时间：" + DateTime.Now.ToString( ) + " 修改前：" + befor + " 修改后：" + after;
        }

        private void linkLabel2_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
        {
            HslCommunication.BasicFramework.FormSupport form = new HslCommunication.BasicFramework.FormSupport( );
            form.ShowDialog( );
        }

        private void button3_Click( object sender, EventArgs e )
        {
            // 信任客户端配置
            using (FormTrustedClient form = new FormTrustedClient( busTcpServer ))
            {
                form.ShowDialog( );
            }
        }
    }
}
