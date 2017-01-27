namespace Votify.Rocks.Service.MailTemplates
{
    public interface IMailTemplate
    {
        string Template { get; set; }
        string Body { get; set; }
    }
}
