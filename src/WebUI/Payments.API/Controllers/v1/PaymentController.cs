using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.Common.Models;
using Payments.Application.Payments.Commands.CreatePayment;
using Payments.Application.Payments.Commands.DeletePayment;
using Payments.Application.Payments.Commands.UpdatePayment;
using Payments.Application.Payments.Queries.GetPayment;
using Payments.Application.Payments.Queries.GetPaymentsList;
using Payments.Application.Payments.Queries.GetPaymentsWithPagination;
using System.Threading.Tasks;

namespace Payments.API.Controllers.v1
{
    /// <summary>
    /// 
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PaymentsController : ApiControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet("pagination")]
        public async Task<ActionResult<PaginationResponse<PaymentDto>>> GetPaymentsWithPagination([FromQuery]GetPaymentsWithPaginationQuery query)
        {
            return await Mediator.Send(query);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PaymentsListVm>> GetAll()
        {
            return await Mediator.Send(new GetPaymentsListQuery());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentVm>> Get(long id)
        {
            return await Mediator.Send(new GetPaymentQuery { Id = id });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<long>> Create(CreatePaymentCommand command)
        {
            var id = await Mediator.Send(command);

            return Created(nameof(Get), id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(long id, UpdatePaymentCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await Mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(long id)
        {
            await Mediator.Send(new DeletePaymentCommand() { Id = id });

            return NoContent();
        }
    }
}