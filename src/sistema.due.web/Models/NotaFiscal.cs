using FluentValidation;
using FluentValidation.Results;
using Sistema.DUE.Web.Helpers;
using System;
using System.Collections.Generic;

namespace Sistema.DUE.Web.Models
{
    public class NotaFiscal : AbstractValidator<NotaFiscal>
    {
        public NotaFiscal()
        {
            NotasReferenciadas = new List<NotaFiscal>();
        }

        public int Id { get; set; }

        public string TipoNF { get; set; }

        public string ChaveNF { get; set; }

        public int Item { get; set; }

        public string NumeroNF { get; set; }

        public string CnpjNF { get; set; }

        public decimal QuantidadeNF { get; set; }

        public string UnidadeNF { get; set; }

        public string NCM { get; set; }

        public string ChaveNFReferencia { get; set; }

        public string Arquivo { get; set; }

        public string DUE { get; set; }

        public int Usuario { get; set; }

        public string Login { get; set; }

        public DateTime? DataNF { get; set; }

        public string Memorando { get; set; }

        public string AnoMemorando { get; set; }

        public string Empresa { get; set; }

        public string Filial { get; set; }

        public string ChaveAcesso { get; set; }

        public string OBS { get; set; }

        public string DataEmissao { get; set; }

        public decimal SaldoCCT { get; set; }

        public decimal SaldoOutrasDUES { get; set; }

        public int DueId { get; set; }

        public string Status { get; set; }

        public string CodSituacao { get; set; }

        public decimal VMLE { get; set; }

        public decimal VMCV { get; set; }

        public int Enquadramento { get; set; }

        public string Recinto { get; set; }

        public List<NotaFiscal> NotasReferenciadas { get; set; }

        public void AdicionarNotasReferenciadas(IEnumerable<NotaFiscal> notasReferenciadas)
        {
            if (notasReferenciadas != null)
                NotasReferenciadas.AddRange(notasReferenciadas);
        }

        public ValidationResult Validar()
        {
            RuleFor(c => c.TipoNF)
                .NotEmpty().WithMessage("Nenhum tipo de nota niscal especificado (EXP, REM, FDL)")
                .Length(3).WithMessage("Tipo de nota fiscal inválido");

            RuleFor(c => c.ChaveNF)
                .NotEmpty().WithMessage("Nenhum número de NF especificado")
                .MinimumLength(35).MaximumLength(44).WithMessage("Número da DANFE inválido.");

            RuleFor(c => c.ChaveNFReferencia)
                .MinimumLength(35).MaximumLength(44).WithMessage($"Número da Chave Referenciada {this.ChaveNFReferencia} inválida. Deve conter entre 36 - 44 dígitos")
                .When(c => c.ChaveNFReferencia != string.Empty);

            RuleFor(c => c.NumeroNF)
                .NotEmpty().WithMessage("Nenhum número de Nota Fiscal especificado")
                .Must(StringHelpers.IsNumero).WithMessage("Número da NF inválido.");

            RuleFor(c => c.CnpjNF)
                .NotEmpty().WithMessage("Nenhum número de CNPJ especificado");

            RuleFor(c => c.UnidadeNF)
                .NotEmpty().WithMessage("Nenhuma unidade de medida especificada");

            //RuleFor(c => c.NCM)
            //    .NotEmpty().WithMessage("Nenhum Código NCM especificado");

            return Validate(this);
        }
    }
}