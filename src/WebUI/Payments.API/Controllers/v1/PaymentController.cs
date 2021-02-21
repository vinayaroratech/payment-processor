using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Payments.Application.Payments.Commands.CreatePayment;
using Payments.Application.Payments.Commands.DeletePayment;
using Payments.Application.Payments.Commands.UpdatePayment;
using Payments.Application.Payments.Queries.GetPayment;
using Payments.Application.Payments.Queries.GetPaymentsList;
using System.Threading.Tasks;

namespace Payments.API.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public class PaymentsController : ApiControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<PaymentsListVm>> Get()
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
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update(UpdatePaymentCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(DeletePaymentCommand command)
        {
            await Mediator.Send(command);

            return NoContent();
        }
    }
}