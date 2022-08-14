using ControleDeUsuarioAccess.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControleDeUsuarioAccess.BusinessRule
{
    public class RegraDeNegocio
    {
        Context<tb_usuario> usuario = new Context<tb_usuario>();

        public List<tb_usuario> ListarUsuario()
        {
            var itens = usuario.Listar();

            return itens;
        }

        public string InserirUsuario(tb_usuario item)
        {
            string mensagem = "";

            usuario.Inserir(item, out mensagem);

            return mensagem;
        }
    }
}
