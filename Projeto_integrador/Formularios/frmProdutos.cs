using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Data.SqlClient;

namespace Projeto_integrador
{
    public partial class frmProdutos : Form
    {
        SqlConnection conexao = new SqlConnection(Properties.Settings.Default.dbCrud1ConnectionString);

        public frmProdutos(Boolean dark)
        {
            InitializeComponent();
            if (dark == true)
            {
                BackColor = Color.FromArgb(18, 18, 18);
                ForeColor = Color.FromArgb(246, 247, 248);
            }
        }
        private void Sqlcmd()
        {
            try
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO produtos VALUES (@cod, @nome, @valor, @data, @tipo)", conexao);
                cmd.Parameters.Add("@cod", SqlDbType.VarChar).Value = txtCod.Text;
                cmd.Parameters.Add("@nome", SqlDbType.VarChar).Value = txtNome.Text;
                cmd.Parameters.Add("@valor", SqlDbType.Decimal).Value = txtValor.Text;
                cmd.Parameters.Add("@data", SqlDbType.SmallDateTime).Value = txtData.Text;
                cmd.Parameters.Add("@tipo", SqlDbType.VarChar).Value = cbmTipo.Text;
                conexao.Open();
                DataTable dataTable = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dataTable);
                MessageBox.Show("CADASTRO FEITO COM SUCESSO", "CADASTRO", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }
        private void txtCod_TextChanged(object sender, EventArgs e)
        {
            txtCod.Text = Regex.Replace(txtCod.Text, "[^0-9]", "k");
        }

        private void estoque()
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO estoque VALUES (@cod, @val, @qtd)", conexao);
            cmd.Parameters.Add("cod", SqlDbType.Int).Value = txtCod.Text;
            cmd.Parameters.Add("@val", SqlDbType.Decimal).Value = txtValor.Text;
            cmd.Parameters.Add("qtd", SqlDbType.Int).Value = txtQtd.Text;

            conexao.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }
        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            if ((txtCod.Text != "") && (txtNome.Text != "") && (txtValor.Text != "") && (txtData.Text != "") && (cbmTipo.Text != "" && (txtQtd.Text != "")))
            {
                Sqlcmd();
                estoque();
            }
            else
            {
                MessageBox.Show("PREENCHA TODOS OS CAMPOS CORRETAMENTE", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtValor_Leave(object sender, EventArgs e)
        {
            try
            {
                float.Parse(txtValor.Text);
            } catch
            {
                MessageBox.Show("INSIRA UM VALOR CORRETO", "PREÇO INVÁLIDO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmProdutos_Load(object sender, EventArgs e)
        {
            txtData.Text = DateTime.Now.Date.ToString();
        }
    }
}

