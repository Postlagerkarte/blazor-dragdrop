using Microsoft.VisualStudio.TestTools.UnitTesting;
using Blazor.DragDrop.Core;
using System.Linq;
using System;
using System.Xml.Serialization;

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
            service.RegisterDropzone(2, new DropzoneOptions() { MaxItems = 1 });

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
        public void Should_AllowSwappingInSameDropzone_And_MaxItems()
        {
            var service = new DragDropService(null);

            service.RegisterDropzone(1, new DropzoneOptions() { MaxItems = 2 });

            var draggable1 = new DraggableItem(service) { Id = 1, DropzoneId = 1 };
            var draggable2 = new DraggableItem(service) { Id = 2, DropzoneId = 1 };


            service.RegisterDraggableForDropzone(draggable1);
            service.RegisterDraggableForDropzone(draggable2);

            Assert.AreEqual(0, draggable1.OrderPosition);
            Assert.AreEqual(1, draggable2.OrderPosition);


            service.ActiveItem = draggable1;
            service.SwapOrInsert(2);

            Assert.AreEqual(0, draggable2.OrderPosition);
            Assert.AreEqual(1, draggable1.OrderPosition);

        }


        [TestMethod]
        public void Should_Limit_Dropzone_To_MaxItems_AllowSwap()
        {
            var service = new DragDropService(null);

            service.RegisterDropzone(1, new DropzoneOptions() { });
            service.RegisterDropzone(2, new DropzoneOptions() { MaxItems = 1, AllowSwap = true });

            var draggable1 = new DraggableItem(service) { Id = 1, DropzoneId = 1, OriginDropzoneId = 1 };
            var draggable2 = new DraggableItem(service) { Id = 2, DropzoneId = 1, OriginDropzoneId = 1 };

            service.RegisterDraggableForDropzone(draggable1);
            service.RegisterDraggableForDropzone(draggable2);

            service.ActiveItem = draggable1;
            service.DropActiveItem(2);

            service._lastDraggedOverItem = draggable1;

            service.ActiveItem = draggable2;
            service.DropActiveItem(2);

            var result = service.GetDraggablesForDropzone(2).Single();

            Assert.AreEqual(draggable2.Id, result.Id);

        }

        [TestMethod]
        public void Should_Limit_Dropzone_To_MaxItems_AllowSwap_NoDraggedOverItem()
        {
            var service = new DragDropService(null);

            service.RegisterDropzone(1, new DropzoneOptions() { });
            service.RegisterDropzone(2, new DropzoneOptions() { MaxItems = 1, AllowSwap = true });

            var draggable1 = new DraggableItem(service) { Id = 1, DropzoneId = 1, OriginDropzoneId = 1 };
            var draggable2 = new DraggableItem(service) { Id = 2, DropzoneId = 1, OriginDropzoneId = 1 };

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

        [TestMethod]
        public void Should_Execute_AcceptFuncOfDropzone_AcceptTest()
        {
            var service = new DragDropService(null);

            service.RegisterDropzone(1, new DropzoneOptions() { });

            service.RegisterDropzone(2, new DropzoneOptions() { Accepts = (d) => d.Name == "Testme" });

            var draggable1 = new DraggableItem(service) { Id = 1, DropzoneId = 1, Tag = new { Name = "Testme" } };

            service.RegisterDraggableForDropzone(draggable1);

            service.ActiveItem = draggable1;
            service.DropActiveItem(2);

            var result = service.GetDraggablesForDropzone(2);

            Assert.AreEqual(1, result.Count);

        }

        [TestMethod]
        public void Should_GetDropzoneByName()
        {
            var service = new DragDropService(null);

            service.RegisterDropzone(1, new DropzoneOptions() { Name = "Dropzone1" });

            service.RegisterDropzone(2, new DropzoneOptions() { Name = "Dropzone2" });

            var draggable1 = new DraggableItem(service) { Id = 1, DropzoneId = 1 };

            service.RegisterDraggableForDropzone(draggable1);

            service.ActiveItem = draggable1;
            service.DropActiveItem(2);

            var result = service.GetDraggablesForDropzone("Dropzone2");

            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(result[0].Id, 1);

        }


        [TestMethod]
        public void Should_ReturnTrue_HasDropzoneDraggables()
        {
            var service = new DragDropService(null);

            service.RegisterDropzone(1, new DropzoneOptions() { Name = "Dropzone1" });

            service.RegisterDropzone(2, new DropzoneOptions() { Name = "Dropzone2" });

            var draggable1 = new DraggableItem(service) { Id = 1, DropzoneId = 1 };

            service.RegisterDraggableForDropzone(draggable1);

            service.ActiveItem = draggable1;
            service.DropActiveItem(2);

            var result = service.HasDropzoneDraggables("Dropzone2");

            Assert.AreEqual(true, result);

        }

        [TestMethod]
        public void Should_ReturnFalse_HasDropzoneDraggables()
        {
            var service = new DragDropService(null);

            service.RegisterDropzone(1, new DropzoneOptions() { Name = "Dropzone1" });

            service.RegisterDropzone(2, new DropzoneOptions() { Name = "Dropzone2" });

            var draggable1 = new DraggableItem(service) { Id = 1, DropzoneId = 1 };

            service.RegisterDraggableForDropzone(draggable1);

            service.ActiveItem = draggable1;
            service.DropActiveItem(2);

            var result = service.HasDropzoneDraggables("Dropzone1");

            Assert.AreEqual(false, result);

        }

        [TestMethod]
        public void Should_ReturnFalse_HasDropzoneDraggables_GivenInvalidName()
        {
            var service = new DragDropService(null);

            service.RegisterDropzone(1, new DropzoneOptions() { Name = "Dropzone1" });

            service.RegisterDropzone(2, new DropzoneOptions() { Name = "Dropzone2" });

            var draggable1 = new DraggableItem(service) { Id = 1, DropzoneId = 1 };

            service.RegisterDraggableForDropzone(draggable1);

            service.ActiveItem = draggable1;

            service.DropActiveItem(2);

            Assert.IsFalse(service.HasDropzoneDraggables("Dropzone3"));

        }

        [TestMethod]
        public void Should_DropActiveItemAndCallOnDrop_DifferentDropzones()
        {
            var service = new DragDropService(null);

            dynamic isDelegateCalled = new { };

            service.RegisterDropzone(1, new DropzoneOptions() { });
            service.RegisterDropzone(2, new DropzoneOptions() { });

            var draggable = new DraggableItem(service)
            {
                Id = 1,
                DropzoneId = 1,
                Tag = new { Test = "OnDropTagTest" },
                OnDrop = (d, i) => isDelegateCalled = d
            };

            service.RegisterDraggableForDropzone(draggable);

            service.ActiveItem = draggable;

            service.DropActiveItem(2);

            var result = service.GetDraggablesForDropzone(2).Single();

            Assert.AreEqual(draggable.Id, result.Id);

            Assert.AreEqual("OnDropTagTest", isDelegateCalled.Test);

        }

        [TestMethod]
        public void Should_DropActiveItemAndCallOnDrop_SameDropzones()
        {
            var service = new DragDropService(null);

            dynamic isDelegateCalled = new { };

            service.RegisterDropzone(1, new DropzoneOptions() { });

            var draggable = new DraggableItem(service)
            {
                Id = 1,
                DropzoneId = 1,
                Tag = new { Test = "OnDropTagTest" },
                OnDrop = (d, i) => isDelegateCalled = d
            };

            service.RegisterDraggableForDropzone(draggable);

            service.ActiveItem = draggable;

            service.DropActiveItem(1);

            Assert.AreEqual("OnDropTagTest", isDelegateCalled.Test);

        }

        [TestMethod]
        public void Should_RemoveDraggableItem()
        {
            var service = new DragDropService(null);

            dynamic isDelegateCalled = new { };

            service.RegisterDropzone(1, new DropzoneOptions() { });

            var draggable = new DraggableItem(service)
            {
                Id = 1,
                DropzoneId = 1,
                Tag = new { Test = "OnDropTagTest" },
                OnDrop = (d, i) => isDelegateCalled = d
            };

            service.RegisterDraggableForDropzone(draggable);

            Assert.IsTrue(service.HasDropzoneDraggables(1));

            service.RemoveDraggableItem(draggable);

            Assert.IsFalse(service.HasDropzoneDraggables(1));

        }

        [TestMethod]
        public void Should_ClearActiveItemUponDropInSameDropzone()
        {
            var service = new DragDropService(null);

            service.RegisterDropzone(1, new DropzoneOptions() { });

            service.RegisterDropzone(2, new DropzoneOptions() { });

            var draggable1 = new DraggableItem(service)
            {
                Id = 1,
                DropzoneId = 2,
            };

            var draggable2 = new DraggableItem(service)
            {
                Id = 2,
                DropzoneId = 2,
            };

            service.ActiveItem = draggable1;

            service.DropActiveItem(2);

            Assert.IsTrue(service.ActiveItem == null);

        }


        [TestMethod]
        public void Should_CheckAllowDrag_True()
        {
            var service = new DragDropService(null);

            var draggable1 = new DraggableItem(service)
            {
                Id = 1,
                DropzoneId = 1,
                Tag = new { Test = true },
                AllowDrag = (d) => d.Tag.Test
            };

            var isDragable = service.IsDraggable(draggable1);

            Assert.AreEqual(true, isDragable);
        }

        [TestMethod]
        public void Should_CheckAllowDrag_False()
        {
            var service = new DragDropService(null);

            var draggable1 = new DraggableItem(service)
            {
                Id = 1,
                DropzoneId = 1,
                Tag = new { Test = false },
                AllowDrag = (d) => d.Tag.Test
            };

            var isDragable = service.IsDraggable(draggable1);

            Assert.AreEqual(false, isDragable);
        }

    }
}
