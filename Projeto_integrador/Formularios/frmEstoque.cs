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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;

namespace Projeto_integrador.Formularios
{
    public partial class frmEstoque : Form
    {
        SqlConnection conexao = new SqlConnection(Properties.Settings.Default.dbCrud1ConnectionString);
        DataTable dt = new DataTable();
        int i;
        public frmEstoque(Boolean dark)
        {
            InitializeComponent();
            if (dark == true)
            {
                BackColor = Color.FromArgb(18, 18, 18);
                ForeColor = Color.FromArgb(246, 247, 248);
            }
        }

       private void registros(int i)
        {
            try
            {
                txtCodigo.Text     = dt.Rows[i][0].ToString();
                txtNome.Text       = dt.Rows[i][1].ToString();
                txtValorUni.Text   = dt.Rows[i][2].ToString();
                txtQuantidade.Text = dt.Rows[i][7].ToString();
            } catch
            {
                MessageBox.Show("SEM RESULTADOS", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void pesquisar(string tabela)
        {
            dt.Reset();
            SqlCommand cmd = new SqlCommand("SELECT * FROM produtos INNER JOIN estoque ON idProduto = cd_prod AND "+tabela+ "= @valor", conexao);
            
            if (tabela == "cd_prod")
            {
                cmd.Parameters.Add("@valor", SqlDbType.Int).Value = txtValor.Text;
 
            } else
            {
                cmd.Parameters.Add("@valor", SqlDbType.Decimal).Value = txtValor.Text;
            }
            conexao.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            try
            {
                da.Fill(dt);
            } catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } finally
            {
                conexao.Close();
            }
            registros(i);
        }
        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            if (cbmProcurar.Text != "" && txtValor.Text != "")
            {

                if (cbmProcurar.Text == "CÓDIGO")
                {
                    pesquisar("cd_prod");
                } else             
                    pesquisar("val_uni");
            } else
            {
                MessageBox.Show("PREENCHA TODOS OS CAMPOS CORRETAMENTE", "AVISO", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            SqlCommand exEstoque = new SqlCommand("DELETE FROM estoque WHERE cd_prod = @valor", conexao);
            exEstoque.Parameters.Add("@valor", SqlDbType.Int).Value = txtCodigo.Text;

            SqlCommand exProduto = new SqlCommand("DELETE FROM produtos WHERE idProduto = @valor", conexao);
            exProduto.Parameters.Add("@valor", SqlDbType.Int).Value = txtCodigo.Text;

            SqlDataAdapter exE = new SqlDataAdapter(exEstoque);
            SqlDataAdapter exP = new SqlDataAdapter(exProduto);


            if (DialogResult.Yes == MessageBox.Show("DESEJA MESMO EXCLUIR ESTE PRODUTO", "CONFIRMAÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
            {
                exE.Fill(dt);
                exP.Fill(dt);
                MessageBox.Show("CLIENTRE EXCLUIDO!", "SUCESSO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            SqlCommand exEstoque = new SqlCommand("UPDATE estoque SET val_uni = @val, qnt_prod = @qtd WHERE cd_prod = @id", conexao);

            exEstoque.Parameters.Add("@id", SqlDbType.Int).Value = txtCodigo.Text;
            exEstoque.Parameters.Add("@val", SqlDbType.Decimal).Value = txtValorUni.Text;
            exEstoque.Parameters.Add("@qtd", SqlDbType.Int).Value = txtQuantidade.Text;

 


            SqlCommand exProduto = new SqlCommand("UPDATE produtos SET nome_pr = @nome," +
                                                                      "val_uni = @val " +
                                                                      "WHERE idProduto = @id", conexao);
            exProduto.Parameters.Add("@id", SqlDbType.Int).Value = txtCodigo.Text;
            exProduto.Parameters.Add("@nome", SqlDbType.VarChar).Value = txtNome.Text;
            exProduto.Parameters.Add("@val", SqlDbType.Decimal).Value = txtValorUni.Text;

     

            SqlDataAdapter exE = new SqlDataAdapter(exEstoque);
            SqlDataAdapter exP = new SqlDataAdapter(exProduto);

           try
           {
                if (DialogResult.Yes == MessageBox.Show("DESEJA MESMO EDITAR ESTE PRODUTO", "CONFIRMAÇÃO", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    exE.Fill(dt);
                    exP.Fill(dt);
                    MessageBox.Show("PRODUTO EDITADO!", "SUCESSO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
           } catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
           }

        }

        private void txtValor_TextChanged(object sender, EventArgs e)
        {
            if (cbmProcurar.Text == "CÓDIGO")
                txtValor.Text = Regex.Replace(txtValor.Text, "[^0-9]", "");
        }

        private void frmEstoque_Load(object sender, EventArgs e)
        {

        }
    }
}

