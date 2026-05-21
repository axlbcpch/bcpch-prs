using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PRS.Models;

[Table("users", Schema = "hris")]
public class User
{
    [Key]
    [Column("trx_id")]
    public int TrxId { get; set; }

    [Column("user_psl_id")]
    public int? UserPslId { get; set; }

    [Column("user_username")]
    public string UserUsername { get; set; } = "";

    [Column("user_password")]
    public string UserPassword { get; set; } = "";
}