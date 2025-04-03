using System;
using System.IO;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;

namespace WpfApp1
{
    class DatosYHorasWriter
    {

        string ruta = Environment.CurrentDirectory;
        string name = "test.docx";

        public DatosYHorasWriter(string nuevaRuta, string nuevoNombre)
        {
            if (!string.IsNullOrWhiteSpace(nuevaRuta))
            {
                ruta = nuevaRuta;
            }
            if (!string.IsNullOrWhiteSpace(nuevoNombre))
            {
                name = nuevoNombre + ".docx";
            }
        }

        public DatosYHorasWriter(string nuevoNombre)
        {
            if (!string.IsNullOrWhiteSpace(nuevoNombre))
            {
                name = nuevoNombre;
            }
        }

        public void WriteToDoc(DatosYHoras[] dyh)
        {
            Document doc = new Document();
            Section section = doc.AddSection();

            Paragraph para = section.AddParagraph();

            for (int i = 0; i < dyh.Length; i++)
            {
                dyh[i].ParseInfo();

                TextRange tr = para.AppendText(dyh[i].Nombre);
                tr.CharacterFormat.Bold = true;
                para.AppendText(dyh[i].info);
            }

            SetStyle(doc, para);

            doc.Protect(ProtectionType.AllowOnlyReading, "");

            //string path = Path.Combine(ruta, name);
            doc.SaveToFile(name, FileFormat.Docx2013);
        }

        public void WriteToDoc(DatosYHoras dyh)
        {
            Document doc = new Document();
            Section section = doc.AddSection();

            Paragraph para = section.AddParagraph();

            dyh.ParseInfo();

            TextRange tr = para.AppendText(dyh.Nombre);
            tr.CharacterFormat.Bold = true;
            para.AppendText(dyh.info);

            SetStyle(doc, para);

            //string path = Path.Combine(ruta, name);
            doc.SaveToFile(name, FileFormat.Docx2013);
        }

        void SetStyle(Document doc, Paragraph para)
        {
            //Create a style for body paragraphs
            ParagraphStyle style = new ParagraphStyle(doc);
            style.Name = "paraStyle";
            style.CharacterFormat.FontName = "Times New Roman";
            style.CharacterFormat.FontSize = 12;
            doc.Styles.Add(style);
            para.ApplyStyle("paraStyle");

            //Set the horizontal alignment of paragraphs
            para.Format.HorizontalAlignment = HorizontalAlignment.Justify;

            //Set the first line indent 
            para.Format.FirstLineIndent = 30;

            //Set the after spacing
            para.Format.AfterSpacing = 8;
        }

    }
}
