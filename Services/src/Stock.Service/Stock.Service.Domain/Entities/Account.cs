using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Stock.Service.Domain.Enums;

namespace Stock.Service.Domain.Entities
{

  //  [Node(
  //IdField = nameof(Id),
  //NodeResolverType = typeof(AccountNodResolver),
  //NodeResolver = nameof(AccountNodResolver.ResolveAsync))]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Type is required")]
        public TypeOfAccount Type { get; set; }
        public string Description { get; set; }

        [ForeignKey("OwnerId")]
        public Guid OwnerId { get; set; }
        public Owner Owner { get; set; }
    }



}
