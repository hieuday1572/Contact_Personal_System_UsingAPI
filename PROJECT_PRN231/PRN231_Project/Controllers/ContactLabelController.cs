using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231_Project.Dto;
using PRN231_Project.Interfaces;
using PRN231_Project.Models;
using PRN231_Project.Repositories;

namespace PRN231_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ContactLabelController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IContact_LabelRepository _contact_LabelRepository;
        public ContactLabelController(IMapper mapper, IContact_LabelRepository contact_LabelRepository)
        {
            _mapper = mapper;
            _contact_LabelRepository = contact_LabelRepository;
        }

        [HttpPost]
        public IActionResult CreateContactLabel([FromBody] Contact_LabelDto contact_labelCreate)
        {
            var contact_labelMap = _mapper.Map<ContactLabel>(contact_labelCreate);
            _contact_LabelRepository.Create(contact_labelMap);
            return Ok();
        }

        [HttpDelete("{contactId}/{labelId}")]
        public IActionResult DeleteLabel(int contactId, int labelId)
        {
            _contact_LabelRepository.Delete( contactId,  labelId);
            return Ok();
        }

        [HttpGet("{contactId}/{labelId}")]
        public IActionResult GetByContactIdAndLabelId(int contactId, int labelId) 
        {
            var item = _mapper.Map<Contact_LabelDto>(_contact_LabelRepository.GetByContactIdAndLabelId(contactId,labelId));
            if (item.Id == 0)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(item);
        }
    }
}
