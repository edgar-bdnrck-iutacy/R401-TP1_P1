using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSConvertisseur.Controllers;
using WSConvertisseur.Models;

namespace WSConvertisseur.Controllers.Tests
{
    [TestClass()]
    public class DevisesControllerTests
    {
        [TestMethod]
        public void GetById_ExistingIdPassed_ReturnsRightItem()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            // Act
            var result = controller.GetById(1);
            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult"); // Test du type de retour
            Assert.IsNull(result.Result, "Erreur est pas null"); //Test de l'erreur
            Assert.IsInstanceOfType(result.Value, typeof(Devise), "Pas une Devise"); // Test du type du contenu (valeur) du retour
            Assert.AreEqual(new Devise(1, "Dollar", 1.08), (Devise?)result.Value, "Devises pas identiques"); //Test de la devise récupérée
        }

        [TestMethod]
        public void GetById_UnknownIdPassed_ReturnsNotFound()
        {
            // Arrange
            DevisesController controller = new DevisesController();

            // Act
            var result = controller.GetById(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult"); // type retour
            Assert.IsNotNull(result.Result, "Erreur est null"); // doit contenir une erreur
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult), "Pas un NotFoundResult"); // type erreur
            Assert.IsNull(result.Value, "Value n'est pas null"); // pas de devise

            Assert.AreEqual(((NotFoundResult)result.Result).StatusCode, StatusCodes.Status404NotFound, "Pas 404");
        }

        [TestMethod]
        public void GetAll_ReturnsAllItems()
        {
            // Arrange
            DevisesController controller = new DevisesController();

            // Act
            var result = controller.GetAll();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<Devise>), "Pas un IEnumerable<Devise>");

            var list = result.ToList();
            CollectionAssert.AreEqual(
                new List<Devise>
                {
            new Devise(1, "Dollar", 1.08),
            new Devise(2, "Franc Suisse", 1.07),
            new Devise(3, "Yen", 120)
                },
                list,
                "Liste des devises incorrecte"
            );
        }

        [TestMethod]
        public void Post_ValidDevisePassed_ReturnsCreatedAtRoute()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            Devise devise = new Devise(4, "Livre", 0.85);

            // Act
            var result = controller.Post(devise);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult<Devise>");
            Assert.IsNotNull(result.Result, "Result est null");

            Assert.IsInstanceOfType(result.Result, typeof(CreatedAtRouteResult), "Pas un CreatedAtRouteResult");

            CreatedAtRouteResult routeResult = (CreatedAtRouteResult)result.Result;
            Assert.AreEqual(StatusCodes.Status201Created, routeResult.StatusCode, "Pas 201");
            Assert.IsInstanceOfType(routeResult.Value, typeof(Devise), "Value n'est pas une Devise");
            Assert.AreEqual(devise, (Devise?)routeResult.Value, "Devise retournée incorrecte");
        }

        [TestMethod]
        public void Put_IdMismatch_ReturnsBadRequest()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            Devise devise = new Devise(2, "Franc Suisse", 1.07);

            // Act
            var result = controller.Put(999, devise);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult), "Pas un ActionResult");
            Assert.IsInstanceOfType(result, typeof(BadRequestResult), "Pas un BadRequestResult");
            Assert.AreEqual(StatusCodes.Status400BadRequest, ((BadRequestResult)result).StatusCode, "Pas 400");
        }

        [TestMethod]
        public void Put_UnknownId_ReturnsNotFound()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            Devise devise = new Devise(999, "Test", 1.0);

            // Act
            var result = controller.Put(999, devise);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult), "Pas un ActionResult");
            Assert.IsInstanceOfType(result, typeof(NotFoundResult), "Pas un NotFoundResult");
            Assert.AreEqual(StatusCodes.Status404NotFound, ((NotFoundResult)result).StatusCode, "Pas 404");
        }

        [TestMethod]
        public void Put_ExistingId_ReturnsNoContent()
        {
            // Arrange
            DevisesController controller = new DevisesController();
            Devise updated = new Devise(1, "Dollar", 1.10);

            // Act
            var result = controller.Put(1, updated);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult), "Pas un ActionResult");
            Assert.IsInstanceOfType(result, typeof(NoContentResult), "Pas un NoContentResult");
            Assert.AreEqual(StatusCodes.Status204NoContent, ((NoContentResult)result).StatusCode, "Pas 204");

            // Bonus : vérifier que la devise a bien été mise à jour
            var get = controller.GetById(1);
            Assert.IsNull(get.Result, "Erreur pas null");
            Assert.AreEqual(updated, (Devise?)get.Value, "Devise pas mise à jour");
        }

        [TestMethod]
        public void Delete_ExistingIdPassed_ReturnsDeletedItem()
        {
            // Arrange
            DevisesController controller = new DevisesController();

            // Act
            var result = controller.Delete(2);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult<Devise>");
            Assert.IsNull(result.Result, "Erreur est pas null");
            Assert.IsInstanceOfType(result.Value, typeof(Devise), "Pas une Devise");
            Assert.AreEqual(new Devise(2, "Franc Suisse", 1.07), (Devise?)result.Value, "Devise supprimée incorrecte");

            // Bonus : vérifier que la devise n'existe plus
            var after = controller.GetById(2);
            Assert.IsInstanceOfType(after.Result, typeof(NotFoundResult), "Après delete, pas NotFound");
        }

        [TestMethod]
        public void Delete_UnknownIdPassed_ReturnsNotFound()
        {
            // Arrange
            DevisesController controller = new DevisesController();

            // Act
            var result = controller.Delete(999);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ActionResult<Devise>), "Pas un ActionResult<Devise>");
            Assert.IsNotNull(result.Result, "Erreur est null");
            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult), "Pas un NotFoundResult");
            Assert.IsNull(result.Value, "Value n'est pas null");
            Assert.AreEqual(((NotFoundResult)result.Result).StatusCode, StatusCodes.Status404NotFound, "Pas 404");
        }


    }
}