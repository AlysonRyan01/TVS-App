using System.ComponentModel.DataAnnotations;

namespace TVS_App.Domain.Enums;

public enum EProduct
{
    Tv = 1,
    [Display(Name = "Controle remoto")]
    ControleRemoto = 5,
    Som = 6,
    [Display(Name = "Caixa acustica")]
    CaixaAcustica = 10,
    Microondas = 18
}