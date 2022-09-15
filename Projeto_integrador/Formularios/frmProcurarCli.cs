using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace Projeto_integrador.Formularios
{
    public partial class frmProcurarCli : Form
    {
        SqlConnection conexao = new SqlConnection(Properties.Settings.Default.dbCrud1ConnectionString);
        DataTable dt = new DataTable();
        int i = 0;

        public frmProcurarCli(Boolean dark)
        {
            InitializeComponent();
            if (dark == true)
            {
                BackColor = Color.FromArgb(18, 18, 18);
                ForeColor = Color.FromArgb(255, 255, 255);
            }
        }
        private void registros(int i)
        {
            try
            {
                txtCodigo.Text = dt.Rows[i][0].ToString();
                txtNome.Text = dt.Rows[i][1].ToString();
                txtCpf.Text = dt.Rows[i][2].ToString();
                txtTelefone.Text = dt.Rows[i][3].ToString();
                txtEndereco.Text = dt.Rows[i][4].ToString();
                cbmUf.Text = dt.Rows[i][5].ToString();
                txtCep.Text = dt.Rows[i][6].ToString();
                txtData.Text = dt.Rows[i][7].ToString();
                txtResp.Text = dt.Rows[i][8].ToString();

            }
            catch
            {
                MessageBox.Show("SEM RESULTADOS");
            }
        }
        private void pesquisa(string tabela)
        {
            dt.Reset();
            SqlCommand cmd = new SqlCommand("SELECT * FROM clientes WHERE " + tabela + " = @valor", conexao);

            if (tabela == "nome")
            {
                cmd.Parameters.Add("@valor", SqlDbType.VarChar).Value = txtValor.Text;
            }
            else
            {
                cmd.Parameters.Add("@valor", SqlDbType.Int).Value = txtValor.Text;
            }
            conexao.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
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
            registros(i);
        }
        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (cbmProcurar.Text != "" && txtValor.Text != "")
            {
                if (cbmProcurar.Text == "NOME")
                {
                    pesquisa("nome");
                }
                else
                {
                    pesquisa("id");
                }
            } else
            {
                MessageBox.Show("DIGITE TODOS OS CAMPOS CORRETAMENTE", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void btnProximo_Click(object sender, EventArgs e)
        {
            i += 1;
            if (i > dt.Rows.Count - 1)
            {
                i = dt.Rows.Count - 1;
            }
            registros(i);
        }

        private void btnAnterior_Click(object sender, EventArgs e)
        {
            i -= 1;
            if (i < 0)
            {
                i = 0;
            }
            registros(i);

        }

        private void button1_Click(object sender, EventArgs e) // BOTAO EXCLUIR
        {
            SqlCommand excluir = new SqlCommand("DELETE FROM clientes WHERE id = @valor", conexao);
            excluir.Parameters.Add("@valor", SqlDbType.Int).Value = txtCodigo.Text;
            SqlDataAdapter ex = new SqlDataAdapter(excluir);


            if (DialogResult.Yes == MessageBox.Show("DESEJA MESMO EXCLUIR ESTE USUÁRIO", "CONFIRMAÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                ex.Fill(dt);
                MessageBox.Show("CLIENTRE EXCLUIDO!", "SUCESSO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            SqlCommand editar = new SqlCommand("UPDATE clientes SET " +
                                                                           "nome = @nome," +
                                                                            "cpf = @cpf," +
                                                                       "telefone = @telefone," +
                                                                       "endereco = @endereco," +
                                                                             "uf = @uf," +
                                                                            "cep = @cep," +
                                                                           "data = @data," +
                                                                           "resp = @resp WHERE id = @id", conexao);
            editar.Parameters.Add("@nome", SqlDbType.VarChar).Value = txtNome.Text;
            editar.Parameters.Add("@cpf", SqlDbType.VarChar).Value = txtCpf.Text;
            editar.Parameters.Add("@telefone", SqlDbType.VarChar).Value = txtTelefone.Text;
            editar.Parameters.Add("@endereco", SqlDbType.VarChar).Value = txtEndereco.Text;
            editar.Parameters.Add("@uf", SqlDbType.VarChar).Value = cbmUf.Text;
            editar.Parameters.Add("@cep", SqlDbType.VarChar).Value = txtCep.Text;
            editar.Parameters.Add("@data", SqlDbType.SmallDateTime).Value = txtData.Text;
            editar.Parameters.Add("@resp", SqlDbType.VarChar).Value = txtResp.Text;
            editar.Parameters.Add("@id", SqlDbType.Int).Value = txtCodigo.Text;

            conexao.Open();

            SqlDataAdapter ed = new SqlDataAdapter(editar);

            try
            {
                ed.Fill(dt);
                MessageBox.Show("CADASTRO MODIFICADO", "SUCESSO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } finally
            {
                conexao.Close();
            }
        }

        private void frmProcurarCli_Load(object sender, EventArgs e)
        {

        }
    }
    }

