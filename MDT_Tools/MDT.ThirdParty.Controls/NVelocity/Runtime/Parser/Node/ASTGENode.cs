namespace NVelocity.Runtime.Parser.Node
{
	/*
	* The Apache Software License, Version 1.1
	*
	* Copyright (c) 2000-2001 The Apache Software Foundation.  All rights
	* reserved.
	*
	* Redistribution and use in source and binary forms, with or without
	* modification, are permitted provided that the following conditions
	* are met:
	*
	* 1. Redistributions of source code must retain the above copyright
	*    notice, this list of conditions and the following disclaimer.
	*
	* 2. Redistributions in binary form must reproduce the above copyright
	*    notice, this list of conditions and the following disclaimer in
	*    the documentation and/or other materials provided with the
	*    distribution.
	*
	* 3. The end-user documentation included with the redistribution, if
	*    any, must include the following acknowlegement:
	*       "This product includes software developed by the
	*        Apache Software Foundation (http://www.apache.org/)."
	*    Alternately, this acknowlegement may appear in the software itself,
	*    if and wherever such third-party acknowlegements normally appear.
	*
	* 4. The names "The Jakarta Project", "Velocity", and "Apache Software
	*    Foundation" must not be used to endorse or promote products derived
	*    from this software without prior written permission. For written
	*    permission, please contact apache@apache.org.
	*
	* 5. Products derived from this software may not be called "Apache"
	*    nor may "Apache" appear in their names without prior written
	*    permission of the Apache Group.
	*
	* THIS SOFTWARE IS PROVIDED ``AS IS'' AND ANY EXPRESSED OR IMPLIED
	* WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
	* OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
	* DISCLAIMED.  IN NO EVENT SHALL THE APACHE SOFTWARE FOUNDATION OR
	* ITS CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
	* SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
	* LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF
	* USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
	* ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
	* OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT
	* OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF
	* SUCH DAMAGE.
	* ====================================================================
	*
	* This software consists of voluntary contributions made by many
	* individuals on behalf of the Apache Software Foundation.  For more
	* information on the Apache Software Foundation, please see
	* <http://www.apache.org/>.
	*/
	using System;
	using Parser = NVelocity.Runtime.Parser.Parser;
	using MethodInvocationException = NVelocity.Exception.MethodInvocationException;
	using InternalContextAdapter = NVelocity.Context.InternalContextAdapter;
	
	public class ASTGENode:SimpleNode
	{
		public ASTGENode(int id):base(id)
		{
		}
		
		public ASTGENode(Parser p, int id):base(p, id)
		{
		}
		
		/// <summary>Accept the visitor. *
		/// </summary>
		public override System.Object jjtAccept(ParserVisitor visitor, System.Object data)
		{
			return visitor.visit(this, data);
		}
		
		public override bool evaluate(InternalContextAdapter context)
		{
			/*
			*  get the two args
			*/
			
			System.Object left = jjtGetChild(0).value_Renamed(context);
			System.Object right = jjtGetChild(1).value_Renamed(context);
			
			/*
			*  if either is null, lets log and bail
			*/
			
			if (left == null || right == null)
			{
				rsvc.error((left == null?"Left":"Right") + " side (" + jjtGetChild((left == null?0:1)).literal() + ") of '>=' operation has null value." + " Operation not possible. " + context.CurrentTemplateName + " [line " + Line + ", column " + Column + "]");
				return false;
			}
			
			/*
			*  if not an Integer, not much we can do either
			*/
			
			if (!(left is System.Int32) || !(right is System.Int32))
			{
				rsvc.error((!(left is System.Int32)?"Left":"Right") + " side of '>=' operation is not a valid type. " + " It is a " + (!(left is System.Int32)?left.GetType():right.GetType()) + ". Currently only integers (1,2,3...) and Integer type is supported. " + context.CurrentTemplateName + " [line " + Line + ", column " + Column + "]");
				
				return false;
			}
			
			return ((System.Int32) left) >= ((System.Int32) right);
			
		}
	}
}