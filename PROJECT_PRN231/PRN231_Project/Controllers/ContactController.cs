using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231_Project.Dto;
using PRN231_Project.Interfaces;
using PRN231_Project.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PRN231_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IContactRepository _contactRepository;
        public ContactController(IMapper mapper, IContactRepository contactRepository)
        {
            _mapper = mapper;
            _contactRepository = contactRepository;
        }
        // GET: api/<ValuesController>
        [HttpGet("{userId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Contact>))]
        public IActionResult GetContacts(int userId)
        {
            var contacts = _mapper.Map<List<ContactDto>>(_contactRepository.GetContactsByUserId(userId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(contacts.Count==0)
            {
                return NotFound();
            }
            return Ok(contacts);
        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Contact))]
        [ProducesResponseType(400)]
        public IActionResult GetContactById(int id)
        {
            if (!_contactRepository.ContactExists(id))
                return NotFound();

            var contact = _mapper.Map<ContactDto>(_contactRepository.GetContactById(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(contact);
        }

        [HttpGet("{userId}/{name}")]
        [ProducesResponseType(200, Type = typeof(Contact))]
        [ProducesResponseType(400)]
        public IActionResult GetContactByName(int userId, string name)
        {
            ICollection<ContactDto> contacts;
            contacts = _mapper.Map<ICollection<ContactDto>>(_contactRepository.GetContactByName(userId, name));
            if (contacts.Count == 0)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(contacts);
        }
        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult CreateContact([FromBody] ContactDto contactCreate)
        {
            var contactMap = _mapper.Map<Contact>(contactCreate);
            contactMap.CreatedDate = DateTime.Now;
            contactMap.IsInTrash = false;
            contactMap.VisitedCount = 0;
            _contactRepository.CreateContact(contactMap);
            return Ok();
        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        public IActionResult UpdateContact([FromBody] ContactDto contactUpdate)
        {
            Contact contactMap = _contactRepository.GetContactById(contactUpdate.Id);
            _mapper.Map(contactUpdate,contactMap);
            contactMap.ModifiedDate = DateTime.Now;           
            _contactRepository.UpdateContact(contactMap);
            return Ok();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteContact( int id)
        {
            _contactRepository.DeleteContact(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetLabelsByContact(int id)
        {
            var Labels = _mapper.Map<List<LabelDto>>(_contactRepository.GetLabelsByContact(id));
            if (Labels == null)
            {
                return NotFound();
            }
            else if(Labels.Count == 0)
            {
                return NotFound();
            }
            return Ok(Labels);
        }
    }
}
