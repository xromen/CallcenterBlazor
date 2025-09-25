namespace Callcenter.Shared;

public class SendDeclarationRequestDto
{
    public int DeclarationId { get; set; }
    public int OrganisationId { get; set; }
    public int OperatorLevel { get; set; }
    public int UserToSendId { get; set; }
}