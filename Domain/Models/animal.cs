using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Domain.Models;

[Index("batch_id", Name = "idx_animals_batch")]
[Index("farm_id", "current_status", Name = "idx_animals_farm_status")]
[Index("paddock_id", Name = "idx_animals_paddock")]
[Index("farm_id", "visual_code", Name = "uk_animal_code_farm", IsUnique = true)]
public partial class animal
{
    [Key]
    public long id { get; set; }

    public int farm_id { get; set; }

    [StringLength(20)]
    public string visual_code { get; set; } = null!;

    [StringLength(50)]
    public string? electronic_code { get; set; }

    [StringLength(100)]
    public string? name { get; set; }

    public DateOnly birth_date { get; set; }

    [MaxLength(1)]
    public char sex { get; set; }

    public int? breed_id { get; set; }

    [StringLength(30)]
    public string? color { get; set; }

    public long? mother_id { get; set; }

    public long? father_id { get; set; }

    [StringLength(50)]
    public string? external_mother { get; set; }

    [StringLength(50)]
    public string? external_father { get; set; }

    public int? batch_id { get; set; }

    public int? paddock_id { get; set; }

    public int? category_id { get; set; }

    [StringLength(20)]
    public string? current_status { get; set; }

    [StringLength(20)]
    public string? purpose { get; set; }

    [StringLength(20)]
    public string? origin { get; set; }

    public DateOnly? entry_date { get; set; }

    [Precision(12, 2)]
    public decimal? initial_cost { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? created_at { get; set; }

    [InverseProperty("father")]
    public virtual ICollection<animal> Inversefather { get; set; } = new List<animal>();

    [InverseProperty("mother")]
    public virtual ICollection<animal> Inversemother { get; set; } = new List<animal>();

    [InverseProperty("animal")]
    public virtual ICollection<animal_movement> animal_movements { get; set; } = new List<animal_movement>();

    [ForeignKey("batch_id")]
    [InverseProperty("animals")]
    public virtual batch? batch { get; set; }

    [ForeignKey("breed_id")]
    [InverseProperty("animals")]
    public virtual breed? breed { get; set; }

    [InverseProperty("calf")]
    public virtual ICollection<calving_calf> calving_calves { get; set; } = new List<calving_calf>();

    [InverseProperty("mother")]
    public virtual ICollection<calving> calvings { get; set; } = new List<calving>();

    [ForeignKey("category_id")]
    [InverseProperty("animals")]
    public virtual animal_category? category { get; set; }

    [ForeignKey("farm_id")]
    [InverseProperty("animals")]
    public virtual farm farm { get; set; } = null!;

    [ForeignKey("father_id")]
    [InverseProperty("Inversefather")]
    public virtual animal? father { get; set; }

    [InverseProperty("animal")]
    public virtual ICollection<feeding_event> feeding_events { get; set; } = new List<feeding_event>();

    [InverseProperty("animal")]
    public virtual ICollection<health_event> health_events { get; set; } = new List<health_event>();

    [InverseProperty("animal")]
    public virtual ICollection<milk_production> milk_productions { get; set; } = new List<milk_production>();

    [ForeignKey("mother_id")]
    [InverseProperty("Inversemother")]
    public virtual animal? mother { get; set; }

    [ForeignKey("paddock_id")]
    [InverseProperty("animals")]
    public virtual paddock? paddock { get; set; }

    [InverseProperty("animal")]
    public virtual ICollection<reproduction_event> reproduction_eventanimals { get; set; } = new List<reproduction_event>();

    [InverseProperty("reproducer")]
    public virtual ICollection<reproduction_event> reproduction_eventreproducers { get; set; } = new List<reproduction_event>();

    [InverseProperty("animal")]
    public virtual ICollection<transaction_animal_detail> transaction_animal_details { get; set; } = new List<transaction_animal_detail>();

    [InverseProperty("animal")]
    public virtual ICollection<weighing> weighings { get; set; } = new List<weighing>();

    [InverseProperty("animal")]
    public virtual ICollection<withdrawal_period> withdrawal_periods { get; set; } = new List<withdrawal_period>();
}
