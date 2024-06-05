using DeviceId;
using System;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

internal class CheckDocs
{
    public void StartCheck()
    {
        try
        {
            string output = Md5Encode(new DeviceIdBuilder().AddMachineName().AddProcessorId()
                           .AddMotherboardSerialNumber().AddSystemDriveSerialNumber().ToString());
            WebClient client = new WebClient();
            string req = client.DownloadString("https://docs.google.com/");
            if (req.Contains(output))
            {
                string data = req.Split(new[] { output }, StringSplitOptions.None)[1].Split('<')[0];
                string[] dataCheck = data.Split('|');
                System.TimeSpan timeSpan = DateTime.ParseExact(dataCheck[1],
                                      "dd/MM/yyyy",
                                      CultureInfo.InvariantCulture).Subtract(DateTime.Now.Date);

                int days = (int)Math.Ceiling(timeSpan.TotalDays);
                if (days <= 0)
                {
                    MessageBox.Show("Vui lòng liên hệ admin để gia hạn.", "Phần mềm hết hạn" + days, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    Environment.Exit(0);
                }
                else
                {
                    //MessageBox.Show("Đăng Nhập Thành Công", "Còn lại: " + days + " ngày!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }

            }
            else
            {
                MessageBox.Show(string.Format("Bạn chưa có bản quyền tool, vui lòng bấm OK và gửi mã \"{0}\" cho chúng tôi để kích hoạt tool!", output), "Thông báo active bản quyền!", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                Clipboard.SetText(output);
                MessageBox.Show("Copy Keys Thành Công.", "Thành Công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                Environment.Exit(0);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi : " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            Environment.Exit(0);
        }
    }

    public static string Md5Encode(string sChuoi)
    {
        MD5 mD = MD5.Create();
        byte[] array = mD.ComputeHash(Encoding.UTF8.GetBytes(sChuoi));
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i++)
        {
            stringBuilder.Append(array[i].ToString("X2"));
        }
        return stringBuilder.ToString();
    }
}
