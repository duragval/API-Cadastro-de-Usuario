using AutoMapper;
using CadastroUsuarioWebApi.Models;

namespace CadastroUsuarioWebApi.DTOs;

public class CadastroDTOMappingProfile : Profile
{
    public CadastroDTOMappingProfile()
    {
        CreateMap<Cadastro,CadastroDTO>().ReverseMap();
    }
}
