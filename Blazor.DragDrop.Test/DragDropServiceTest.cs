using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blazor.DragDrop.Core;
using System.Linq;

namespace Blazor.DragDrop.Test
{
    [TestClass]
    public class DragDropServiceTest
    {
        [TestMethod]
        public void Should_GetDraggablesForDropzone_NoDraggables()
        {
            var service = new DragDropService(null);

            service.RegisterDropzone(1, new DropzoneOptions() { });

            var result = service.GetDraggablesForDropzone(1);

            Assert.AreEqual(0, result.Count);

        }

        [TestMethod]
        public void Should_GetDraggablesForDropzone_OneDraggable()
        {
            var service = new DragDropService(null);

            service.RegisterDropzone(1, new DropzoneOptions() { });

            service.RegisterDraggableForDropzone(new DraggableItem(service) { DropzoneId = 1 });

            var result = service.GetDraggablesForDropzone(1);

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void Should_DropActiveItem()
        {
            var service = new DragDropService(null);

            service.RegisterDropzone(1, new DropzoneOptions() { });
            service.RegisterDropzone(2, new DropzoneOptions() { });

            var draggable = new DraggableItem(service) { Id = 1, DropzoneId = 1 };

            service.RegisterDraggableForDropzone(draggable);

            service.ActiveItem = draggable;

            service.DropActiveItem(2);

            var result = service.GetDraggablesForDropzone(2).Single();

            Assert.AreEqual(draggable.Id, result.Id);

        }

        [TestMethod]
        public void Should_Limit_Dropzone_To_MaxItems()
        {
            var service = new DragDropService(null);

            service.RegisterDropzone(1, new DropzoneOptions() { });
            service.RegisterDropzone(2, new DropzoneOptions() {MaxItems = 1 });

            var draggable1 = new DraggableItem(service) { Id = 1, DropzoneId = 1 };
            var draggable2 = new DraggableItem(service) { Id = 2, DropzoneId = 1 };

            service.RegisterDraggableForDropzone(draggable1);
            service.RegisterDraggableForDropzone(draggable2);

            service.ActiveItem = draggable1;
            service.DropActiveItem(2);

            service.ActiveItem = draggable2;
            service.DropActiveItem(2);

            var result = service.GetDraggablesForDropzone(2).Single();

            Assert.AreEqual(draggable1.Id, result.Id);

        }

        [TestMethod]
        public void Should_Execute_AcceptFuncOfDropzone_RejectTest()
        {
            var service = new DragDropService(null);

            service.RegisterDropzone(1, new DropzoneOptions() { });

            service.RegisterDropzone(2, new DropzoneOptions() { Accepts = (d) => false });

            var draggable1 = new DraggableItem(service) { Id = 1, DropzoneId = 1 };

            service.RegisterDraggableForDropzone(draggable1);

            service.ActiveItem = draggable1;
            service.DropActiveItem(2);

            var result = service.GetDraggablesForDropzone(2);

            Assert.AreEqual(0, result.Count);

        }

    }
}
