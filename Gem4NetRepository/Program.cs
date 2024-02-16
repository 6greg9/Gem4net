using Gem4netRepository.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
namespace Gem4netRepository;
public class Program
{
    public Program()
    {
        using var db = new GemDbContext();
        

    }
}