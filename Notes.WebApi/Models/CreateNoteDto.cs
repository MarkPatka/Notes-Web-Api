﻿using AutoMapper;
using Notes.Application.Common.Mappings;
using Notes.Application.Notes.Commands.CreateNote;
using System.ComponentModel.DataAnnotations;

namespace Notes.WebApi.Models;

public class CreateNoteDto : IMapWith<CreateNoteCommand>
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile.CreateMap<CreateNoteDto, CreateNoteCommand>()
            .ForMember(noteCommand => noteCommand.Title, 
                opt => opt.MapFrom(noteDto => noteDto.Title))
            .ForMember(noteCommand => noteCommand.Details, 
                opt => opt.MapFrom(noteDto => noteDto.Details));
    }
}




