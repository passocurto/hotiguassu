using System;
using System.Collections;
using System.Data;
using System.Web;
using System.Data.SqlClient;
using System.IO;
using System.Text;


namespace hotiguassu.Models
{
	public class ModuloGeral
	{
		
		public static string GetIPAddress(HttpRequestBase request)
		{
			string ipRequisicao = "" + request.ServerVariables["HTTP_X_FORWARDED_FOR"];

			// se não tem proxy pega o ip address padrão
			if (((string.IsNullOrEmpty(ipRequisicao)) || (ipRequisicao.ToLower() == "unknown")))
			{
				ipRequisicao = request.ServerVariables["REMOTE_ADDR"];
			}
			else
			{
				ipRequisicao = "Proxy: " + request.ServerVariables["REMOTE_ADDR"] + Environment.NewLine + " Interno: " + ipRequisicao;
			}
			return ipRequisicao;
		}

        public static string gerarChave(int size)
        {
            Random rnd = new Random();
            string skey = "";
            for (int i = 0; i < size; i++)
            {
                skey += rnd.Next(0, 9).ToString();
            }
            return skey;

        }

        public static byte[] encrypt(string pwd)
        {
            System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(pwd);

            return md5.ComputeHash(inputBytes);
        }

        public static bool comparepwd(byte[] pwd1, byte[] pwd2)
        {
            bool bEqual = false; //passinho se vc mexer aqui novamente corto seus dedos!!! Giliardi 14/05/2012
            if (pwd1.Length == pwd2.Length)
            {
                int i = 0;
                while ((i < pwd1.Length) && (pwd1[i] == pwd2[i]))
                {
                    i += 1;
                }
                if (i == pwd1.Length)
                {
                    bEqual = true;
                }
            }
            return bEqual;
        }

        internal static string trataSqlException( Exception ex)
        {
            if (ex is SqlException)
            {
                SqlException sex = ex as SqlException;
                if (sex.Number == 547) // foreign key
                    return string.Format("Não é possivel alterar o registro do arquivo pois ele necessita de registros não informados do banco de dados.");
                else
                    if (sex.Number == 8152) // string or binary number
                        return "Não é possivel alterar o registro do arquivo selecionado pois há campos com comprimento superior ao especificado no banco de dados.\nVerifique campos com textos longos e reduza seu conteúdo.";
                    else
                        return ex.ToString();

            }else{

                return ex.ToString();
            }
        }

        public static string carregaTemplateEmail(string caminhoTemplateEmail)
        {
            var sr = new StreamReader(caminhoTemplateEmail, System.Text.Encoding.Default);
            var conteudo = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            return conteudo;
        }

        /// <summary>
        /// Registra o log do erro no arquivo.
        /// </summary>
        /// <param name="logFile">The filename to log to</param>
        /// <param name="text">The message to log</param>
        public static void WriteLog(string logFile, string text)
        {
            try
            {
                //TODO: Format nicer
                StringBuilder message = new StringBuilder();
                message.AppendLine(DateTime.Now.ToString());
                message.AppendLine(text);
                message.AppendLine("=========================================");

                System.IO.File.AppendAllText(logFile, message.ToString());
            }
            catch (Exception ex)
            {

            }
        }
	}
}