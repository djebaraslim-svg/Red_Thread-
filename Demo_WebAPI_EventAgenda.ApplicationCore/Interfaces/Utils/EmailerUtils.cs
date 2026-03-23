using Demo_WebAPI_EventAgenda.Domain.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Demo_WebAPI_EventAgenda.ApplicationCore.Interfaces.Utils
{
    public class EmailerUtils : IEmailerUtils
    {
        public void SendEmail(Member member)
        {
            // Configs
            string smtpHost = "localhost";
            int smtpPort = 25;
            string emailApp = "no-reply@agendaevent.brussels";
            string appName = "AgendaEvent Brussels";

            // Mail
            // - Création du mail
            MimeMessage message = new MimeMessage();

            // - Informations d'en-tête du message (expéditeur, destinataire, sujet)
            message.From.Add(new MailboxAddress("San Goku", "no-reply@agendaevent.brussels"));
            message.To.Add(new MailboxAddress("Vladimir", "v.brasseur@bruxellesformation.brussels"));
            message.Subject = "Bienvenue jeune EventPlanner";

            // - Création du corps du message (ajout du contenu du message)
            BodyBuilder bodyBuilder = new BodyBuilder();
            //bodyBuilder.TextBody = "This is a demo email sent using MailKit. (Vieux Client)";
            bodyBuilder.HtmlBody = @"
            <div>
                <h1> Bienvenue sur EventAgenda Brussels</h1>
                <p> Votre compte d'inscription a bien été enregistrée! </p> 
                <p> Bonne journée! </p> 
            </div>";

            message.Body = bodyBuilder.ToMessageBody();

            // Envoi du mail
            using (SmtpClient smtpClient = new SmtpClient ())
            {

                try
                {
                    //- Connexion au serveur SMTP
                    smtpClient.Connect(smtpHost, smtpPort, false);

                    //- Authentification (si nécessaire)
                    smtpClient.Authenticate("username", "password");

                    //- Envoi du mail
                    smtpClient.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }

                finally //Toujours se déconnecter du serveur SMTP que le mail soit envoyé ou pas!
                {
                    //- Déconnexion du serveur SMTP
                    smtpClient.Disconnect(true);
                }
            }

        }

        
    }

}
