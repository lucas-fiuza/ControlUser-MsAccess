using ControleDeUsuarioAccess.BusinessRule;
using ControleDeUsuarioAccess.Connection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControleDeUsuarioAccess
{
    public partial class Form1 : Form
    {
        RegraDeNegocio regra = new RegraDeNegocio();

        public Form1()
        {
            InitializeComponent();
        }

        private void btnInserir_Click(object sender, EventArgs e)
        {

            string id = txtId.Text;
            string nome = txtNome.Text;
            string sobrenome = txtSobrenome.Text;

            if (!string.IsNullOrEmpty(id) && !string.IsNullOrEmpty(nome) && !string.IsNullOrEmpty(sobrenome))
            {
                tb_usuario usuario = new tb_usuario()
                {
                    Id = long.Parse(id),
                    Nome = nome,
                    Sobrenome = sobrenome
                };

                string mensagem = regra.InserirUsuario(usuario);

                MessageBox.Show(mensagem, "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                txtId.Text = "";
                txtNome.Text = "";
                txtSobrenome.Text = "";

                CarregarDados();
            }
            else
            {
                MessageBox.Show("Todos os campos devem ser preenchidos", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            CarregarDados();
        }

        private void CarregarDados()
        {
            dgv_usuairo.DataSource = regra.ListarUsuario();
        }
    }
}
