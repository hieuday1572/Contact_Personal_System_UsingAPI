using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PRN231_Project.Dto;
using PRN231_Project.Interfaces;
using Label = PRN231_Project.Models.Label;

namespace PRN231_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILabelRepository _labelRepository;
        public LabelController(IMapper mapper, ILabelRepository labelRepository)
        {
            _mapper = mapper;
            _labelRepository = labelRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Label>))]
        public IActionResult GetLabels()
        {
            var labels = _mapper.Map<List<LabelDto>>(_labelRepository.GetLabels());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (labels.Count == 0)
            {
                return NotFound();
            }
            return Ok(labels);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(Label))]
        [ProducesResponseType(400)]
        public IActionResult GetLabelById(int id)
        {
            var item = _mapper.Map<LabelDto>(_labelRepository.GetLabel(id));
            if (item.Id == 0)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(item);
        }
        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult CreateLabel([FromBody] LabelDto labelCreate)
        {
            var labelMap = _mapper.Map<Label>(labelCreate);
            _labelRepository.Create(labelMap);
            return Ok();
        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        public IActionResult UpdateLabel([FromBody] LabelDto labelUpdate)
        {
            var labelMap = _mapper.Map<Label>(labelUpdate);
            _labelRepository.Update(labelMap);
            return Ok();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteLabel(int id)
        {
            _labelRepository.Delete(id);
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetContactByLabel(int id)
        {
            var contacts = _mapper.Map<List<ContactDto>>(_labelRepository.GetContactsByLabel(id));
            return Ok(contacts);
        }
    }
}
