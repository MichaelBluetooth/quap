using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Quap.Models;
using Quap.Models.DTO;

namespace Quap.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TagsController : ControllerBase
    {
        private readonly QuapDbContext _ctx;
        private readonly IMapper _mapper;
        private readonly ILogger<TagsController> _logger;

        public TagsController(QuapDbContext ctx, IMapper mapper, ILogger<TagsController> logger)
        {
            _ctx = ctx;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult<TagDetail> GetById([FromRoute] Guid id)
        {
            Tag tag = _ctx.Tags.FirstOrDefault(t => t.id == id);
            if (null == tag)
            {
                return NotFound();
            }
            else
            {
                return Ok(_mapper.Map<TagDetail>(tag));
            }
        }

        [HttpGet]
        public ActionResult<ICollection<TagDetail>> Get()
        {
            return Ok(_ctx.Tags.ProjectTo<TagDetail>(_mapper.ConfigurationProvider));
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult Delete([FromRoute] Guid id)
        {
            Tag tag = _ctx.Tags.Find(id);
            if (null == tag)
            {
                return NotFound();
            }
            foreach (QuestionTag doomed in _ctx.QuestionTags.Where(qt => qt.tagId == id))
            {
                _ctx.QuestionTags.Remove(doomed);
            }
            _ctx.Tags.Remove(tag);
            _ctx.SaveChanges();
            return NoContent();
        }

        [HttpPost]
        public ActionResult<TagDetail> Post([FromBody] TagCreateOrUpdate req)
        {
            Tag newTag = _mapper.Map<Tag>(req);
            _ctx.Tags.Add(newTag);
            _ctx.SaveChanges();
            return CreatedAtAction(nameof(GetById), new { id = newTag.id }, _mapper.Map<TagDetail>(newTag));
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult<TagDetail> Put(Guid id, [FromBody] TagCreateOrUpdate req)
        {
            Tag existing = _ctx.Tags.Find(id);
            if (null != existing)
            {
                existing.name = req.name;
                existing.description = req.description;
                _ctx.Entry(existing).State = EntityState.Modified;
                _ctx.SaveChanges();
                return Ok(_mapper.Map<TagDetail>(existing));
            }
            else
            {
                return NotFound();
            }
        }
    }
}
