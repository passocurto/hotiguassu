using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Text;


namespace hotiguassu.Models
{
    [Table("tbSisNotificacoesEmail")]
    public class NotificacaoEmail
    {

        [Key]
        public int idNotificacao { get; set; }

        [Display(Name = "Data Notificação")]
        public DateTime? dtNotificacao { get; set; }

        [Display(Name = "Objeto")]
        public string dsObjeto { get; set; }

        [Display(Name = "id Objeto")]
        public int? idObjeto { get; set; }

        [Display(Name = "Descrição")]
        public string dsNotificacao { get; set; }

        [Display(Name = "Erro")]
        public string dsErro { get; set; }

        [Display(Name = "Enviado")]
        public bool? flEnviado { get; set; }


        public void notificaEmail(hotiguassuContext db, string pathTemplates)
        {
        
            try
            {
                bool emailEnviado = true; 
                switch (this.dsObjeto)
                {
                    case "N":
                        this.notificaCadastro(db, "");
                        break;
                }
                this.dsErro = "";
                this.flEnviado = emailEnviado;
            }
            catch (Exception ex)
            {
                var erro = ex.ToString();
                if (erro.Length > 500)
                    this.dsErro = erro.Substring(0, 500);
                else
                    this.dsErro = erro;
            }

        }



        public void notificaCadastro(hotiguassuContext db, string pathTemplates)
        {

            var idGirls = this.idObjeto ?? 0;
            var Girl = db.GirlsModels.FirstOrDefault(c => c.idGirl == idGirls);
            if (Girl != null)
            {
                MailMessage mail = new MailMessage(new MailAddress("naoresponda@hotiguassu.com", "hotiguassu.com"), new MailAddress(Girl.email));
                mail.Subject = "Confirmação de registro.";

                string strEmail = ModuloGeral.carregaTemplateEmail(pathTemplates + "\\confirmacaoRegistro.txt");
                strEmail = strEmail.Replace("@idCliente", "" + Girl.idGirl);
                

                mail.IsBodyHtml = true;
                mail.Body = strEmail;

                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);
            }
        }

      

    }
}