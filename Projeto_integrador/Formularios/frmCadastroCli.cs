using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Projeto_integrador
{
    public partial class frmCadastroCli : Form
    {
        SqlConnection conexao = new SqlConnection(Properties.Settings.Default.dbCrud1ConnectionString);
   
        public frmCadastroCli(Boolean dark)
        {
            InitializeComponent();
            if(dark == true)
            {
                BackColor = Color.FromArgb(18, 18, 18);
                ForeColor = Color.FromArgb(246, 247, 248);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            //var principal = new frmPaginaPrincipal();
            //principal.Show();
        }
        private void Sqlcmd(string resp)
        {
            try
            {
                    SqlCommand cmd = new SqlCommand("INSERT INTO clientes VALUES (@nome, @cpf, @telefone, @endereco, @uf, @cep, @data, @resp)", conexao);
                    cmd.Parameters.Add("@nome", SqlDbType.VarChar).Value = txtNome.Text;
                    cmd.Parameters.Add("@cpf", SqlDbType.VarChar).Value = txtCpf.Text;
                    cmd.Parameters.Add("@telefone", SqlDbType.VarChar).Value = txtTelefone.Text;
                    cmd.Parameters.Add("@endereco", SqlDbType.VarChar).Value = txtEndereco.Text;
                    cmd.Parameters.Add("@uf", SqlDbType.VarChar).Value = cbmUf.Text;
                    cmd.Parameters.Add("@cep", SqlDbType.VarChar).Value = txtCep.Text;
                    cmd.Parameters.Add("@data", SqlDbType.SmallDateTime).Value = txtData.Text;
                    cmd.Parameters.Add("@resp", SqlDbType.VarChar).Value = resp;

                    conexao.Open();
                    DataTable dataTable = new DataTable();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dataTable);
                    MessageBox.Show("CADASTRO FEITO COM SUCESSO", "CADASTRO", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } finally
            {
                conexao.Close();
            }
        }
        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if ((txtNome.Text.Length > 1) && (txtCpf.Text.Length == 14) && (txtTelefone.Text.Length == 14) && (txtEndereco.Text.Length > 1) && (cbmUf.Text != "") && (txtCep.Text.Length == 9) && (txtData.Text.Length == 10))
            {
                Sqlcmd(txtResponsavel.Text);
            } else
            {
                MessageBox.Show("DIGITE TODOS OS CAMPOS CORRETAMENTE", "ALGUM CAMPO INVÁLIDO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtData_Leave(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToDateTime(txtData.Text).AddYears(18) < DateTime.Now)
                {
                    labelRes.Visible = false;
                    txtResponsavel.Visible = false;
                    txtResponsavel.Clear();
                }
                else
                {
                    MessageBox.Show("DIGITE NOME DE UM RESPONSÁVEL", "MENOR DE IDADE", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    labelRes.Visible = true;
                    txtResponsavel.Visible = true;
                    txtResponsavel.Clear();
                }
            } catch 
            {
                MessageBox.Show("DIGITE UMA DATA CORRETA", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(txtCpf.Text);
        }

        private void txtEndereco_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmCadastroCli_Load(object sender, EventArgs e)
        {
            
        }
    }
}

