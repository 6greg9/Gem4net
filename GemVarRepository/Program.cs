using GemVarRepository.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
namespace GemVarRepository;
public class Program
{
    public Program()
    {
        using var db = new GemDbContext();
        

    }
}