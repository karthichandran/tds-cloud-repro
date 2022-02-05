using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReProServices.Application.Customers;
using ReProServices.Application.Customers.Commands.CreateCustomer;
using ReProServices.Application.Customers.Commands.DeleteCustomer;
using ReProServices.Application.Customers.Commands.UpdateCustomer;
using ReProServices.Application.Customers.Queries;
using ReProServices.Domain.Entities;
using WeihanLi.Npoi;

namespace WebApi.Controllers
{
    [Authorize]
    public class CustomerController : ApiController
    {
        [Authorize(Roles = "Client_View")]
        [HttpGet]
        public async Task<CustomerVM> Get([FromQuery]CustomerDetailsFilter customerDetailsFilter)
        {
            return await Mediator.Send(new GetCustomersQuery() { Filter = customerDetailsFilter});
        }
        [Authorize(Roles = "Client_View")]
        [HttpGet("getCustomerCount")]
        public async Task<CustomerCountDto> GetCustomerCount()
        {
            return await Mediator.Send(new GetCustomerCountQuery() {  });
        }
        [Authorize(Roles = "Client_View")]
        [HttpGet("getExcel")]
        public async Task<FileResult> GetExcel([FromQuery]CustomerDetailsFilter customerDetailsFilter)
        {
            var result = await Mediator.Send(new GetCustomersQuery() { Filter = customerDetailsFilter });
            var resultSet = result.customersView;

            var settings = FluentSettings.For<ViewCustomerPropertyBasic>();
            settings.HasAuthor("REpro Services");
            
            settings.Property(_ => _.CustomerName)
                .HasColumnTitle("Customer Name")
                .HasColumnWidth(50)
                .HasColumnIndex(0);

            settings.Property(x => x.PAN)
                .HasColumnWidth(16)
                .HasColumnIndex(1);

            settings.Property(x => x.PropertyPremises)
                .HasColumnTitle("Property Premises")
                .HasColumnWidth(30)
                .HasColumnIndex(2);

            settings.Property(x => x.UnitNo)
                .HasColumnTitle("Unit No")
                .HasColumnWidth(18)
                .HasColumnIndex(3);

            settings.Property(x => x.TotalUnitCost)
                .HasColumnTitle("Unit Cost")
                .HasColumnWidth(18)
                .HasColumnIndex(4);

            settings.Property(x => x.DateOfAgreement)
                .HasColumnTitle("Date of Agreement")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(18)
                .HasColumnIndex(5);

            settings.Property(x => x.DateOfSubmission)
                .HasColumnTitle("Date of Submission")
                .HasColumnFormatter("dd-MMM-yyy")
                .HasColumnWidth(18)
                .HasColumnIndex(6);

            settings.Property(x => x.Remarks)
                .HasColumnTitle("Remarks")
                .HasColumnWidth(60)
                .HasColumnIndex(7);

            settings.Property(x => x.TracesPassword)
              .HasColumnTitle("Traces Password")
              .HasColumnWidth(60)
              .HasColumnIndex(8);

            settings.Property(x => x.CustomerAlias)
             .HasColumnTitle("Alias")
             .HasColumnWidth(60)
             .HasColumnIndex(9);

            settings.Property(_ => _.OwnershipID).Ignored();
            settings.Property(_ => _.CustomerID).Ignored();
            settings.Property(_ => _.PropertyID).Ignored();
            settings.Property(_ => _.CustomerPropertyID).Ignored();
            settings.Property(_ => _.OwnershipID).Ignored();
            settings.Property(_ => _.PaymentMethodId).Ignored();
            settings.Property(_ => _.StatusTypeID).Ignored();

            var ms = resultSet.ToExcelBytes();

            return File(ms, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "CustomerDetails.xls");

        }
        [Authorize(Roles = "Client_View")]
        [HttpGet("{id}")]
        public async Task<CustomerVM> GetById(Guid id)
        {
            return await Mediator.Send(new GetCustomerByIDQuery { OwnershipId = id });
        }

        [HttpGet("PAN/{pan}")]
        public async Task<CustomerDto> GetByPAN(string pan)
        {
            return await Mediator.Send(new GetCustomerByPANQuery { PAN = pan });
        }
        [Authorize(Roles = "Client_Create")]
        [HttpPost]
        public async Task<ActionResult<CustomerVM>> Create(CreateCustomerCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }
        [Authorize(Roles = "Client_Edit")]
        [HttpPut()]
        public async Task<ActionResult<CustomerVM>> Update(UpdateCustomerCommand command)
        {
            var result = await Mediator.Send(command);
            return result;
        }

        [HttpDelete("{id}/{ownershipid}")]
        public async Task<Unit> Delete(int id,Guid ownershipId) {
            return await Mediator.Send(new DeleteCustomerCommand { CustomerID = id ,OwnershipID=ownershipId});
        }
    }
}