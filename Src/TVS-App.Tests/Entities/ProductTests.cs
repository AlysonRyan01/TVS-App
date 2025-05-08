using TVS_App.Domain.Entities;
using TVS_App.Domain.Enums;
using TVS_App.Domain.Exceptions;
using TVS_App.Domain.ValueObjects.Product;

namespace TVS_App.Tests.Entities;

[TestClass]
public class ProductTests
{
    [TestMethod]
    public void UpdateProduct_deve_atualizar_todos_os_valores()
    {
        var product = new Product(
            new Model("UN40J5200AG"),
            new SerialNumber("ABC123456"),
            new Defect("Tela quebrada"),
            "Cabo, Controle",
            EProduct.Tv
        );

        product.UpdateProduct(
            "UN43J5200AG",
            "XYZ987654",
            "Sem som",
            "Cabo USB",
            EProduct.Tv
        );

        Assert.AreEqual("UN43J5200AG", product.Model.ProductModel);
        Assert.AreEqual("XYZ987654", product.SerialNumber.ProductSerialNumber);
        Assert.AreEqual("Sem som", product.Defect!.ProductDefect);
        Assert.AreEqual("Cabo USB", product.Accessories);
        Assert.AreEqual(EProduct.Tv, product.Type);
    }

    [TestMethod]
    public void UpdateProduct_deve_lancar_excecao_quando_model_for_vazio()
    {
        var product = new Product(
            new Model("UN40J5200AG"),
            new SerialNumber("ABC123456"),
            new Defect("Tela quebrada"),
            "CABO",
            EProduct.Tv
        );

        Assert.ThrowsException<ValueObjectException<Model>>(() =>
        {
            product.UpdateProduct("", "XYZ", "Defeito", "Acessório", EProduct.Tv);
        });
    }
}